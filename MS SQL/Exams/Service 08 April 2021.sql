CREATE TABLE Status(
	Id INT PRIMARY KEY IDENTITY,
	Label NVARCHAR(30) NOT NULL
)

CREATE TABLE Departments(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL
)

CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id) NOT NULL
)

CREATE TABLE Employees(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(25),
	LastName NVARCHAR(25),
	Birthdate DATETIME2,
	Age INT,
	CHECK(Age BETWEEN 18 AND 110),
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id) NOT NULL
)	

CREATE TABLE Users(
	Id INT PRIMARY KEY IDENTITY,
	Username NVARCHAR(30) UNIQUE NOT NULL,
	Password NVARCHAR(50) NOT NULL,
	[Name] NVARCHAR(50),
	Birthdate DATETIME2,
	Age INT,
	CHECK(Age BETWEEN 18 AND 110),
	Email NVARCHAR(50) NOT NULL
)

CREATE TABLE Reports(
	Id INT PRIMARY KEY IDENTITY,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	StatusId INT FOREIGN KEY REFERENCES Status(Id) NOT NULL,
	OpenDate DATETIME2 NOT NULL,
	CloseDate DATETIME2,
	[Description]  NVARCHAR(200) NOT NULL, 
	UserId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id)

)

--Section 2. DML (10 pts)
--2.	Insert

INSERT INTO Employees(FirstName,LastName,BirthDate,DepartmentId)
	VALUES
		('Marlo','O''Malley','1958-9-21',1),
		('Niki','Stanaghan','1969-11-26',4),
		('Ayrton','Senna','1960-03-21',9),
		('Ronnie','Peterson','1944-02-14',9),
		('Giovanna','Amati','1959-07-20',5)

INSERT INTO Reports(CategoryId,StatusId,OpenDate,CloseDate,Description,UserId,EmployeeId)
	VALUES
		(1,1,'2017-04-13',NULL,'Stuck Road on Str.133',6,2),
		(6,3,'2015-09-05','2015-12-06','Charity trail running',3,5),
		(14,2,'2015-09-07',NULL,'Falling bricks on Str.58',5,2),
		(4,3,'2017-07-03','2017-07-06','Cut off streetlight on Str.11',1,1)

--3.	Update
SELECT *
FROM Reports
UPDATE Reports 
	SET CloseDate = GETDATE()
	WHERE CloseDate IS NULL

	--UPDATE Reports
	--SET CloseDate = 
	--ISNULL(CloseDate,GETDATE())

--4.Delete

DELETE FROM Reports
WHERE StatusId = 4

--Section 3. Querying (40 pts)
--5.	Unassigned Reports

SELECT Description, FORMAT(OpenDate,'dd-MM-yyyy') AS OpenDate
FROM Reports
WHERE EmployeeId IS NULL  
ORDER BY Reports.OpenDate ASC,[Description] ASC

--6.	SELECT Description, c.Name AS CategoryName

SELECT Description, c.Name AS CategoryName
FROM Reports AS r
JOIN Categories AS c ON c.Id = r.CategoryId
WHERE CategoryId IS NOT NULL
ORDER BY [Description]ASC, CategoryName ASC

--7.	Most Reported Category

SELECT TOP(5) c.[Name],COUNT (*) AS ReportNumber 
FROM Reports AS r
JOIN Categories AS c ON c.Id = r.CategoryId
GROUP BY CategoryId, c.Name
ORDER BY ReportNumber DESC, c.Name ASC

--7.	Most Reported Category

--SELECT TOP(5) Name,
--	(SELECT COUNT(*)FROM Reports WHERE CategoryId = c.Id) AS ReportsNumber
--	FROM Categories AS	c
--	ORDER BY [ReportsNumber] DESC, c.Name ASC

--8.	Birthday Report

SELECT u.UserName,c.Name 
FROM Users AS u
JOIN Reports AS r ON r.UserId = u.Id 
JOIN Categories AS c ON c.Id = r.CategoryId
WHERE Day(r.OpenDate) = Day(u.BirthDate) AND Month(r.OpenDate) = Month(u.BirthDate)
ORDER BY UserName ASC,c.Name

--9.	Users per Employee 

SELECT 
FirstName + ' ' +LastName AS FullName,
COUNT (DISTINCT UserId) AS UsersCount
FROM Reports AS r
RIGHT JOIN Employees AS e ON e.Id = r.EmployeeId
GROUP BY EmployeeId,FirstName, LastName
ORDER BY [UsersCount] DESC,FullName ASC

--9.	Users per Employee 

--SELECT 
--	FirstName + ' ' +LastName AS FullName,
--	(SELECT COUNT (DISTINCT UserId) FROM Reports WHERE EmployeeId = e.Id) AS UsersCount
--FROM Employees AS e
--ORDER BY [UsersCount] DESC, FullName ASC


--10.	Full Info
SELECT 
			CASE
			WHEN e.FirstName IS NULL THEN 'None'
			ELSE CONCAT(e.FirstName,' ',e.LastName)
			END AS Employee,
		--ISNULL(CONCAT(e.FirstName,' ',e.LastName),'None') AS Employee,
		ISNULL(d.Name,'None') AS Department,
		ISNULL(c.[Name],'None') AS Category,
		ISNULL(r.[Description],'None') AS [Description],
		ISNULL(FORMAT(r.OpenDate,'dd.MM.yyyy'),'None') AS OpenDate,
		ISNULL(st.[Label],'None') AS Status,
		ISNULL(u.[Name],'None') AS [User]

		
FROM Reports AS r
LEFT JOIN Employees AS e ON r.EmployeeId = e.Id
LEFT JOIN Departments AS d ON e.DepartmentId = d.Id
LEFT JOIN Categories AS c ON r.CategoryId = c.Id
LEFT JOIN Status AS st ON r.StatusId = st.Id
LEFT JOIN Users AS u ON r.UserId = u.Id
ORDER BY e.FirstName DESC,e.LastName DESC,d.[Name] ASC,c.[Name] ASC,r.[Description] ASC,r.OpenDate ASC,st.Label ASC,u.[Name] ASC


--Section 4. Programmability (20 pts)
--11.	Hours to Complete
GO
CREATE FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT
AS
BEGIN

	IF @StartDate IS NULL RETURN 0
	IF @EndDate IS NULL RETURN 0

	RETURN(DATEDIFF(HOUR,@startDate,@endDate))

END
GO
SELECT dbo.udf_HoursToComplete(OpenDate, CloseDate) AS TotalHours
   FROM Reports


--12.	Assign Employee
GO
CREATE PROCEDURE usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
AS
		DECLARE @departEmployee INT;
		DECLARE @departCategory INT;

		SET @departEmployee = (SELECT DepartmentId FROM Employees WHERE Id = @EmployeeId)

		SET @departCategory = (SELECT DepartmentId FROM Categories AS c
								JOIN Reports AS r ON r.CategoryId = c.Id
								WHERE r.Id = @ReportId)

		IF @departEmployee != @departCategory
			THROW 50003,'Employee doesn''t belong to the appropriate department!',1
		
		ELSE
			UPDATE Reports
			SET EmployeeId = @EmployeeId
			WHERE Id = @ReportId	

			EXEC usp_AssignEmployeeToReport 30, 1
			EXEC usp_AssignEmployeeToReport 17, 2