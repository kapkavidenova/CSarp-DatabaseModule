CREATE TABLE Cities(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(20) NOT NULL,
	CountryCode NCHAR(2) NOT NULL
)

CREATE TABLE Hotels(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(30) NOT NULL,
	CityId INT FOREIGN KEY REFERENCES Cities(Id) NOT NULL,
	EmployeeCount INT NOT NULL,
	BaseRate DECIMAL(18,2)
)

CREATE TABLE Rooms(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	Price DECIMAL(18,2) NOT NULL,
	Type NVARCHAR(20) NOT NULL,
	Beds INT NOT NULL,
	HotelId INT FOREIGN KEY REFERENCES Hotels(Id) NOT NULL

)

CREATE TABLE Trips(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	RoomId INT FOREIGN KEY REFERENCES Rooms(Id) NOT NULL,
	BookDate DateTime NOT NULL,
	CHECK(BookDate<ArrivalDate),
	ArrivalDate DateTime NOT NULL,
	CHECK(ArrivalDate<ReturnDate),
	ReturnDate DATETIME NOT NULL,
	CancelDate DATETIME

)

CREATE TABLE Accounts(
	Id INT PRIMARY KEY IDENTITY,
	FirstName  NVARCHAR(50) NOT NULL,
	MiddleName NVARCHAR(20),
	LastName NVARCHAR(50) NOT NULL,
	CityId INT FOREIGN KEY REFERENCES Cities(Id) NOT NULL,
	BirthDate DATETIME2 NOT NULL,
	Email NVARCHAR(100) UNIQUE NOT NULL
	
)

CREATE TABLE AccountsTrips(
	AccountId  INT FOREIGN KEY REFERENCES Accounts(Id) NOT NULL,
	TripId INT FOREIGN KEY REFERENCES Trips(Id) NOT NULL,
	Luggage INT NOT NULL,
	CHECK(Luggage>=0)

)

--Section 2. DML (10 pts)
--2. Insert
INSERT INTO Accounts
	VALUES
		('John',' Smith','Smith', 34,'1975-07-21','j_smith@gmail.com'),
		('Gosho',NULL,'Petrov', 11,'1978-05-16','g_petrov@gmail.com'),
		('Ivan','Petrovich','Pavlov', 59,'1849-09-26','i_pavlov@softuni.bg'),
		('Friedrich','Wilhelm','Nietzsche',	2,'1844-10-15','f_nietzsche@softuni.bg')
	


		INSERT INTO Trips(RoomId,BookDate,ArrivalDate,ReturnDate,CancelDate)
	VALUES
		(101,'2015-04-12','2015-04-14','2015-04-20','2015-02-02'),
		(102,'2015-07-07','2015-07-15','2015-07-22','2015-04-29'),
		(103,'2013-07-17','2013-07-23','2013-07-24',NULL),
		(104,'2012-03-17','2012-03-31','2012-04-01','2012-01-10'),
		(109,'2017-08-07','2017-08-28','2017-08-29',NULL)


--3. Update
UPDATE Rooms
SET Price = Price * 1.14
WHERE HotelId IN (5,7,9)

--4. Delete
DELETE FROM AccountsTrips
WHERE AccountId = 47

DELETE FROM Accounts
WHERE Id = 47


--Section 3. Querying (40 pts)
--5. EEE-Mails

SELECT acc.FirstName,
		acc.LastName,
		FORMAT(acc.BirthDate,'MM-dd-yyyy'),
		c.[Name],
		acc.Email
FROM Accounts AS acc
JOIN Cities AS c ON acc.CityId = c.Id
WHERE acc.Email LIKE 'e%'
ORDER BY c.[Name] ASC


--6. City Statistics
SELECT c.[Name] AS City,
		COUNT(*) AS Hotels
FROM Cities AS c
RIGHT JOIN Hotels AS h ON c.Id = h.CityId
GROUP BY h.CityId,c.[Name]
ORDER BY [Hotels] DESC,City ASC


--7. Longest and Shortest Trips
--
SELECT AccountId,
		FirstName + ' ' + LastName AS FullName,
		MAX(DATEDIFF(DAY,ArrivalDate,ReturnDate)) AS LongestTrip,
		MIN(DATEDIFF(DAY,ArrivalDate,ReturnDate)) AS ShortestTrip
FROM Trips AS t
JOIN AccountsTrips AS at ON t.Id = at.TripId
JOIN Accounts AS a ON at.AccountId = a.Id
WHERE t.CancelDate IS NULL AND a.MiddleName IS NULL
GROUP BY AccountId,FirstName,LastName
ORDER BY LongestTrip DESC,ShortestTrip ASC


--8. Metropolis
SELECT c.Id,
		c.[Name] AS City,
		c.CountryCode AS Country,
		COUNT(c.Id) AS Accounts
FROM Cities AS c
LEFT JOIN Accounts AS a ON c.Id = a.CityId
GROUP BY c.Id,c.[Name],c.CountryCode 
ORDER BY Accounts DESC

--9. Romantic Getaways
SELECT a.Id,
		a.Email,
		c.[Name],
		COUNT(t.Id) AS Trips
--		select *
FROM Accounts AS a
JOIN AccountsTrips AS at ON a.Id = [at].AccountId
JOIN Trips AS t ON at.TripId = t.Id
JOIN Rooms AS r ON r.Id = t.RoomId
JOIN Hotels AS h ON r.HotelId = h.Id
JOIN Cities AS c ON h.CityId = c.Id
WHERE a.CityId = h.CityId 
GROUP BY a.Id,a.Email,c.[Name]
ORDER BY Trips DESC,a.Id ASC

--10. GDPR Violation

SELECT tr.Id,
		CONCAT(a.FirstName,' ',	ISNULL(a.MiddleName+' ',''),a.LastName) AS [Full Name],
		c.[Name] AS [From],
		cc.[Name] AS [To],
		CASE
			WHEN tr.CancelDate IS NULL THEN CONCAT(DATEDIFF(DAY,tr.ArrivalDate,tr.ReturnDate),' days')
			ELSE 'Canceled'
		END AS [Duration]

--FROM Trips AS t
FROM AccountsTrips AS act
JOIN Accounts AS a ON act.AccountId = a.Id
JOIN Cities AS c ON a.CityId = c.Id
JOIN Trips AS tr ON act.TripId = tr.Id
JOIN Rooms AS r ON tr.RoomId = r.Id
JOIN Hotels AS h ON r.HotelId = h.Id
JOIN Cities AS cc ON h.CityId = cc.Id
ORDER BY [Full Name] ASC,tr.Id ASC

--Section 4. Programmability (14 pts)
--11. Available Room
GO
CREATE OR ALTER FUNCTION  udf_GetAvailableRoom (@HotelId int, @Date DATE, @People int)
RETURNS VARCHAR(200)
AS
BEGIN

	DECLARE @roomId INT = (SELECT TOP(1) r.Id
							FROM Rooms AS r  
							LEFT JOIN Trips AS t ON
								r.Id = t.RoomId
							WHERE r.HotelId = @HotelId
							AND ((@Date NOT BETWEEN t.ArrivalDate AND t.ReturnDate
									AND t.CancelDate IS NOT NULL) OR
								t.ArrivalDate IS NULL OR t.ReturnDate IS NULL OR t.CancelDate IS NULL) 
							AND r.Beds >= @People
							ORDER BY r.Price DESC);

	IF (@roomId IS NULL) RETURN 'No rooms available';


	DECLARE @baseRate DECIMAL(5,2)  = (SELECT BaseRate FROM Hotels WHERE Id = @HotelId);
	DECLARE @roomPrice DECIMAL(18, 2) = (SELECT Price FROM Rooms WHERE Id = @roomId);
	DECLARE @roomType VARCHAR(20) = (SELECT [Type] FROM Rooms WHERE Id = @roomId);
	DECLARE @beds INT = (SELECT Beds FROM Rooms WHERE Id = @roomId);

	DECLARE @totalPrice DECIMAL(18, 2) = (@baseRate + @roomPrice) * @People;

	RETURN ('Room ' + CAST(@roomId AS varchar(3)) + ': ' + @roomType +
			' (' + CAST(@beds AS varchar(3)) + ' beds)'+ ' - $' + CAST(@totalPrice AS varchar(20)))
END

--12. Switch Room
CREATE PROCEDURE usp_SwitchRoom(@TripId INT, @TargetRoomId INT)
AS
DECLARE @currentRoomId INT = (SELECT RoomId FROM Trips WHERE Id = @TripId);
DECLARE @currentRoomHotelId INT = (SELECT HotelId FROM Rooms WHERE Id = @currentRoomId);
DECLARE @targetRoomHotelId INT = (SELECT HotelId FROM Rooms WHERE Id = @TargetRoomId);
DECLARE @accCount INT = (SELECT COUNT (@TripId) FROM AccountsTrips WHERE TripId = @TripId);
DECLARE @targetRoomBeds INT = (SELECT Beds FROM Rooms WHERE Id = @TargetRoomId);

IF(@currentRoomHotelId != @targetRoomHotelId)
	THROW 50001, 'Target room is in another hotel!',1

IF(@accCount > @targetRoomBeds)
	THROW 50002, 'Not enough beds in target room!', 1 

UPDATE Trips
	SET RoomId = @TargetRoomId
	WHERE Id = @TripId