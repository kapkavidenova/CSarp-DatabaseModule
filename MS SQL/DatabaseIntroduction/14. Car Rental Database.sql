--14. Car Rental Database
CREATE DATABASE CarRental

CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY,
	CategoryName NVARCHAR(50) NOT NULL,
	DailyRate DECIMAL(3,1),
	WeeklyRate DECIMAL(3,1),
	MonthlyRate DECIMAL(3,1) NOT NULL,
	WeekendRate DECIMAL(3,1) NOT NULL
)
--SELECT * FROM Categories

CREATE TABLE Cars(
	Id INT PRIMARY KEY IDENTITY,
	PlateNumber NVARCHAR(50),
	Manafacturer NVARCHAR(50) NOT NULL,
	Model NVARCHAR(50) NOT NULL,
	CarYear SMALLINT NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Doors TINYINT NOT NULL,
	Picture VARBINARY(MAX),
	Condition NVARCHAR(50) NOT NULL,
	Available BIT NOT NULL
)

CREATE TABLE Employees(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Title NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Customers(
	Id INT PRIMARY KEY IDENTITY,
	LisenceNumber NVARCHAR(MAX) NOT NULL,
	FullName NVARCHAR(100) NOT NULL,
	[Address] NVARCHAR(100) NOT NULL,
	City NVARCHAR(30) NOT NULL,
	ZIPCode NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE RentalOrders(
	Id INT PRIMARY KEY IDENTITY,
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id),
	CarId INT FOREIGN KEY REFERENCES Cars(Id),
	TankLevel TINYINT NOT NULL,
	KilometrageStart INT NOT NULL,
	KilometrageEnd INT NOT NULL,
	TotalKilometrage INT NOT NULL,
	StartDate DATETIME2 NOT NULL,
	EndDate DATETIME2 NOT NULL,
	TotalDays SMALLINT NOT NULL,
	RateApplied DECIMAL(2,1),
	TaxRate DECIMAL(2,1) NOT NULL,
	OrderStatus BIT NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Categories
	VALUES
		('Family',9.0,NULL, 12,15),
		('Personal',12.0,15,18,19),
		('FamilySmall',5.0,6,6,5.5)
--SELECT * FROM Categories

INSERT INTO Cars(PlateNumber,Manafacturer,Model,CarYear,CategoryId, Doors,Picture,Condition,Available)
	VALUES
		('MZ50','RP','AB',1990,1,4,NULL,'Bad',1),
		('MZ55','RP','AC',1995,2,4,NULL,'Good',1),
		('MZ60','RP','AD',1998,3,4,NULL,'Perfect',0)

--SELECT * FROM Cars

INSERT INTO Employees(FirstName,LastName,Title,Notes)
	VALUES
		('Bob','Dark','Director',NULL),
		('Mark','Smith','Operator',NULL),
		('Jack','Taylor','Specialist',NULL)
--SELECT * FROM Employees

INSERT INTO Customers(LisenceNumber,FullName,[Address],City,ZIPCode,Notes)
	VALUES
		('MZX111222333','Stamo Petkov','Sofia','Sofia','SF1000','THE BEST'),
		('MZX111222334','Ivan Ivanov','Plovdiv','Plovdiv','PD5000',NULL),
		('MZX111222335','Pesho Peshev','Pernik','Pernik','PK2300','NOT FOUND')
--SELECT * FROM Customers

INSERT INTO RentalOrders(CustomerId,CarId,TankLevel,KilometrageStart,KilometrageEnd,	TotalKilometrage, StartDate, EndDate, TotalDays, RateApplied, TaxRate,OrderStatus, Notes)
	VALUES
		(1,1,5,20000,25000,5000,'2020-05-22','2020-05-25',4, 5,9,1,NULL),
		(2,2,5,20000,25000,5000,'2020-05-22','2020-05-25',4, 5,9,0,NULL),
		(3,3,7,20000,25000,5000,'2020-05-22','2020-05-25',4, 5,9,1,NULL)
SELECT * FROM RentalOrders

--SELECT * FROM Cars
--SELECT * FROM Employees
--SELECT * FROM Customers



