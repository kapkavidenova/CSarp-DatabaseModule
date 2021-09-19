--15. Hotel Database
CREATE DATABASE Hotel

CREATE TABLE Employees(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Title NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)
--SELECT * FROM Employees

INSERT INTO Employees(FirstName,LastName,Title,Notes)
	VALUES
		('Bob','Smith','Director',NULL),
		('Mark','Taylor','Specialist',NULL),
		('Jame','Jayson','Manager',NULL)

--SELECT * FROM Employees

CREATE TABLE Customers(
	AccountNumber INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	PhoneNumber NVARCHAR(50) NOT NULL,
	EmergencyName NVARCHAR(50) NOT NULL,
	EmergencyNumber NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

INSERT INTO Customers(FirstName,LastName,PhoneNumber,EmergencyName,EmergencyNumber,Notes)
	VALUES
		('Moni','Monov','111-222-333','Monka','aa11',NULL),
		('Mimi','Ivanova','444-555-666','Mim','bb22',NULL),
		('Mara','Mineva','777-888-999','Mar','cc33',NULL)
--SELECT * FROM Customers

CREATE TABLE RoomStatus(
	RoomStatus NVARCHAR(50) PRIMARY KEY,
	Notes NVARCHAR(MAX)
)

INSERT INTO RoomStatus(RoomStatus,Notes)
	VALUES
		('OPEN','Dont ring!'),
		('OPEN2','Be careful!'),
		('OPEN3',NULL)
--SELECT * FROM RoomStatus

CREATE TABLE RoomTypes(
	RoomType NVARCHAR(50) PRIMARY KEY,
	Notes NVARCHAR(50)
)

INSERT INTO RoomTypes(RoomType,Notes)
	VALUES
		('Relax','Enjoy!'),
		('Work',NULL),
		('Write','Nice')

CREATE TABLE BedTypes(
	BedType NVARCHAR(50) PRIMARY KEY,
	Notes NVARCHAR(MAX)
)

INSERT INTO BedTypes(BedType,Notes)
	VALUES
		('Big',NULL),
		('For children',NULL),
		('Small',NULL)

CREATE TABLE Rooms(
	RoomNumber INT PRIMARY KEY,
	RoomType NVARCHAR(50) FOREIGN KEY REFERENCES RoomTypes(RoomType) NOT NULL,
	BedType NVARCHAR(50) FOREIGN KEY REFERENCES BedTypes(BedType) NOT NULL,
	Rate DECIMAL (3,1) NOT NULL,
	RoomStatus NVARCHAR(50) FOREIGN KEY REFERENCES RoomStatus(RoomStatus) NOT NULL,
	Notes NVARCHAR(MAX)
)

INSERT INTO Rooms(RoomNumber, RoomType,BedType,Rate,RoomStatus,Notes)
	VALUES
		(1,'Relax','Big',5.5,'OPEN', NULL),
		(2,'Work','For children',5.0,'OPEN2', NULL),
		(3,'Write','Small',6.5,'OPEN3', NULL)

CREATE TABLE Payments(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	PaymentDate DATETIME2 NOT NULL,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber) NOT NULL,
	FirstDateOccupied DATETIME2 NOT NULL,
	LastDateOccupied DATETIME2 NOT NULL,
	TotalDays AS DATEDIFF(DAY,FirstDateOccupied,LastDateOccupied),
	AmountCharged DECIMAL(7,2) NOT NULL,
	TaxRate DECIMAL(5,2) NOT NULL,
	TaxAmount AS AmountCharged * TaxRate,
	PaymentTotal DECIMAL(8,2) NOT NULL,
	Notes NVARCHAR(MAX)
)

INSERT INTO Payments(EmployeeId,PaymentDate,AccountNumber,FirstDateOccupied,LastDateOccupied, AmountCharged,TaxRate, PaymentTotal,Notes)
	VALUES
		(1,'05-25-2010',1,'05-24-2010','05-24-2010',200,0.05,210,'DONE'),
		(2,'05-27-2010',1,'05-26-2010','05-26-2010',300,0.05,315,'DONE'),
		(3,'05-28-2010',1,'05-29-2010','05-29-2010',400,0.05,420,'DONE')

--SELECT * FROM Payments

CREATE TABLE Occupancies(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	DateOccupied DATETIME2 NOT NULL,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber),
	RoomNumber INT FOREIGN KEY REFERENCES Rooms(RoomNumber),
	RateApplied DECIMAL(4,2) NOT NULL,
	PhoneCharge DECIMAL(4,2),
	Notes NVARCHAR(MAX)
)	

INSERT INTO Occupancies(EmployeeId,DateOccupied,AccountNumber,RoomNumber,RateApplied,PhoneCharge,Notes)
	VALUES
		(1,'05-24-2010', 1, 1,20,50,NULL),
		(2,'05-27-2010', 2, 2,20.5,40,NULL),
		(3,'05-28-2010', 3, 3,21,30,NULL)
--SELECT * FROM Occupancies