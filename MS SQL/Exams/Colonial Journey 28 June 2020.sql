--USE [ColonialJourney]
CREATE TABLE Planets(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] VARCHAR(30) NOT NULL
)

CREATE TABLE Spaceports(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	PlanetId INT FOREIGN KEY REFERENCES Planets(Id) NOT NULL
)

CREATE TABLE Spaceships(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	Manufacturer VARCHAR(30) NOT NULL,
	LightSpeedRate INT DEFAULT 0
)

CREATE TABLE Colonists(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(20) NOT NULL,
	LastName VARCHAR(20) NOT NULL,
	Ucn VARCHAR(10) UNIQUE NOT NULL,
	BirthDate Date NOT NULL
)


CREATE TABLE Journeys(
	Id INT PRIMARY KEY IDENTITY,
	JourneyStart DATETIME2 NOT NULL,
	JourneyEnd DATETIME2 NOT NULL,
	Purpose VARCHAR(11),
	CHECK (Purpose = 'Medical' OR Purpose = 'Technical' OR Purpose = 'Educational' OR Purpose = 'Military'),
	DestinationSpaceportId INT FOREIGN KEY REFERENCES Spaceports(Id) NOT NULL,
	SpaceshipId  INT FOREIGN KEY REFERENCES Spaceships(Id) NOT NULL

)	

CREATE TABLE TravelCards(
	Id INT PRIMARY KEY IDENTITY,
	CardNumber CHAR(10) UNIQUE NOT NULL,
	JobDuringJourney VARCHAR(8),
	CHECK(JobDuringJourney='Pilot' OR JobDuringJourney='Engineer' OR JobDuringJourney='Trooper' OR JobDuringJourney='Cleaner' OR JobDuringJourney='Cook'),
	ColonistId INT FOREIGN KEY REFERENCES Colonists(Id) NOT NULL,
	JourneyId INT FOREIGN KEY REFERENCES Journeys(Id) NOT NULL

)

--Section 2. DML
--2.	Insert
INSERT INTO Planets
		VALUES
			('Mars		 '),
			('Earth		 '),
			('Jupiter	 '),
			('Saturn	 ')

--SELECT * FROM Planets

INSERT INTO Spaceships
		VALUES
			('Golf	  ',' VW	 ',  3  ),
			('WakaWaka',' Wakanda',	4	),
			('Falcon9'	,'SpaceX',1		),
			('Bed	  ',' Vidolov',	6	)
--SELECT * FROM Spaceships

--3.	Update
--SELECT * FROM Spaceships
UPDATE Spaceships
	SET LightSpeedRate +=1
	WHERE Id BETWEEN 8 AND 12

--4.	Delete
SELECT * FROM Journeys
SELECT * FROM Spaceships
DELETE FROM TravelCards
WHERE JourneyId BETWEEN 1 AND 3

DELETE FROM Journeys
WHERE Id BETWEEN 1 AND 3


--Section 3. Querying 
--5.	Select all military journeys
SELECT Id,
		FORMAT(JourneyStart,'dd/MM/yyyy'),
		FORMAT(JourneyEnd,'dd/MM/yyyy')

FROM Journeys
WHERE Purpose = 'Military'
ORDER BY JourneyStart ASC

--6.	Select all pilots
SELECT c.Id,
		CONCAT(c.FirstName,' ',c.LastName) AS [full_name]
FROM Colonists AS c
RIGHT JOIN TravelCards AS tc ON c.Id = tc.ColonistId
WHERE JobDuringJourney = 'Pilot'
ORDER BY c.Id ASC

--7.	Count colonists
SELECT COUNT(*) AS count
FROM Colonists AS c
RIGHT JOIN TravelCards AS tc ON c.Id = tc.ColonistId
RIGHT JOIN Journeys AS j ON tc.JourneyId = j.Id
WHERE j.Purpose = 'Technical'

--8.	Select spaceships with pilots younger than 30 years
SELECT sp.[Name],
		sp.Manufacturer
FROM Spaceships AS sp
LEFT JOIN Journeys AS j ON sp.Id = j.SpaceshipId
LEFT JOIN TravelCards AS tc ON j.Id = tc.JourneyId
LEFT JOIN Colonists AS c ON tc.ColonistId = c.Id
WHERE tc.JobDuringJourney = 'Pilot' AND (DATEDIFF(year,c.BirthDate,'2019-01-01')<30)
ORDER BY sp.[Name]

--9.	Select all planets and their journey count
SELECT pl.[Name] AS [PlanetName],
		COUNT(*) AS [JourneysCount]
FROM Planets AS pl
LEFT JOIN Spaceports AS sp ON pl.Id = sp.PlanetId
JOIN Journeys AS j ON sp.Id = j.DestinationSpaceportId
GROUP BY pl.[Name]
ORDER BY JourneysCount DESC,pl.[Name] ASC	


--10.	Select Second Oldest Important Colonist
SELECT *
FROM
	(SELECT 
		tc.JobDuringJourney,
		CONCAT(c.FirstName,' ',c.LastName) AS FullName,
		DENSE_RANK() OVER (PARTITION BY tc.JobDuringJourney ORDER BY c.BirthDate) AS [JobRank]

	 FROM Colonists AS c
	 JOIN TravelCards AS tc ON c.Id = tc.ColonistId
	) AS [temp]
WHERE [JobRank]= 2

--Section 4. Programmability
--11.	Get Colonists Count
GO
CREATE FUNCTION dbo.udf_GetColonistsCount(@PlanetName VARCHAR (30)) 
RETURNS INT
AS
BEGIN
	DECLARE @COUNT INT = 
	(SELECT COUNT(tc.ColonistId) 
	FROM Planets AS pl
	LEFT JOIN Spaceports AS sp ON pl.Id = sp.PlanetId
	JOIN Journeys AS j ON sp.Id = j.DestinationSpaceportId
	JOIN TravelCards AS tc ON j.Id = tc.JourneyId
	WHERE pl.[Name] = @PlanetName)

	RETURN @COUNT 

END
GO
SELECT dbo.udf_GetColonistsCount('Otroyphus')


--12.	Change Journey Purpose
GO
CREATE PROCEDURE usp_ChangeJourneyPurpose(@JourneyId INT, @NewPurpose VARCHAR(11))
AS
	DECLARE @existId INT = 
		(SELECT TOP(1) Id FROM Journeys
		WHERE Id = @JourneyId)
	IF @existId IS NULL
		THROW 50001,'The journey does not exist!',1

	DECLARE @existPurpose NVARCHAR(20) = 
		(SELECT TOP(1) Purpose FROM Journeys
		 WHERE Id = @JourneyId)
	IF @existPurpose = @NewPurpose
		THROW 50002,'You cannot change the purpose!', 1

UPDATE Journeys
	SET Purpose = @NewPurpose
	WHERE Id = @JourneyId

	EXEC usp_ChangeJourneyPurpose 2, 'Educational'
	EXEC usp_ChangeJourneyPurpose 4, 'Technical'