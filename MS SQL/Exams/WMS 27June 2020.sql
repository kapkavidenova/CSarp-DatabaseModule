CREATE TABLE Clients(
	
	ClientId INT PRIMARY KEY IDENTITY NOT NULL,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Phone CHAR(12) NOT NULL,

)

CREATE TABLE Mechanics(
	MechanicId INT PRIMARY KEY IDENTITY NOT NULL,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	[Address] VARCHAR(255) NOT NULL,

)

CREATE TABLE Vendors(
	VendorId INT PRIMARY KEY  IDENTITY NOT NULL,
	[Name] VARCHAR(50) UNIQUE ,
)


CREATE TABLE Parts(
	PartId INT PRIMARY KEY IDENTITY NOT NULL,
	SerialNumber VARCHAR(50) UNIQUE,
	[Description] VARCHAR(255),
	Price DECIMAL(6,2) NOT NULL,
	CHECK(Price >0),
	VendorId INT FOREIGN KEY REFERENCES Vendors(VendorId),
	StockQty INT DEFAULT(0),
	CHECK(StockQty >=0)

)

CREATE TABLE Models(
	ModelId INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] VARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Jobs(
	JobId INT PRIMARY KEY IDENTITY NOT NULL,
	ModelId INT FOREIGN KEY REFERENCES Models(ModelId) NOT NULL,
	[Status] VARCHAR(11) DEFAULT('Pending') NOT NULL,
	CHECK(Status IN ('Pending','In Progress','Finished')),
	ClientId INT FOREIGN KEY REFERENCES Clients(ClientId) NOT NULL,
	MechanicId INT FOREIGN KEY REFERENCES Mechanics(MechanicId),
	IssueDate DATE NOT NULL,
	FinishDate DATE NULL 

)

CREATE TABLE Orders(
	OrderId INT PRIMARY KEY IDENTITY NOT NULL,
	JobId INT FOREIGN KEY REFERENCES Jobs(JobId) NOT NULL,
	IssueDate DATE NULL,
	Delivered BIT DEFAULT(0)
	
)

CREATE TABLE OrderParts(
	OrderId  INT FOREIGN KEY REFERENCES Orders(OrderId) NOT NULL,
	PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL,
	PRIMARY KEY(OrderId, PartId),
	Quantity INT DEFAULT(1),
	CHECK(Quantity >0)

)

CREATE TABLE PartsNeeded(
	JobId INT FOREIGN KEY REFERENCES Jobs(JobId) NOT NULL,
	PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL,
	Quantity INT DEFAULT(1) NOT NULL,
	PRIMARY KEY (JobId, PartId),
	CHECK(Quantity>0)
)

--Section 2. DML
--2.	Insert

INSERT INTO Clients(FirstName, LastName, Phone)
VALUES
	('Teri', 'Ennaco', '570-889-5187'),
	('Merlyn', 'Lawler', '201-588-7810'),
	('Georgene', 'Montezuma', '925-615-5185'),
	('Jettie',	'Mconnell',	'908-802-3564'),
	('Lemuel',	'Latzke', '631-748-6479'),
	('Melodie',	'Knipp', '805-690-1682'),
	('Candida',	'Corbley', '908-275-8357')

INSERT INTO Parts(SerialNumber, [Description], Price, VendorId)
VALUES
	('WP8182119',	'Door Boot Seal',	117.86,	2),
	('W10780048',	'Suspension Rod',	42.81,	1),
	('W10841140',	'Silicone Adhesive', 	6.77,	4),
	('WPY055980',	'High Temperature Adhesive',	13.94,	3)

	--3.	Update

UPDATE Jobs	
	SET MechanicId = 3, [Status] = 'In Progress'
	WHERE Status = 'Pending'



--4.	Delete
SELECT * FROM Orders
DELETE FROM OrderParts
WHERE OrderId = 19

DELETE FROM Orders
WHERE OrderId = 19

--Section 3. Querying 
--5.	Mechanic Assignments
SELECT 
	CONCAT(m.FirstName, ' ', m.LastName) AS Mechanic,
	j.[Status],
	j.IssueDate
FROM Mechanics AS m
JOIN Jobs AS j ON
	m.MechanicId = j.MechanicId
ORDER BY m.MechanicId, j.IssueDate, j.JobId

--6.	Current Clients
SELECT 
	CONCAT(c.FirstName, ' ', c.LastName) AS Client,
	DATEDIFF(DAY, j.IssueDate, '2017-04-24') AS [Days going],
	j.[Status]
FROM Clients AS c
JOIN Jobs AS j ON
	c.ClientId = j.ClientId
WHERE j.[Status] != 'Finished'
ORDER BY [Days going] DESC, c.ClientId ASC


--7.	Mechanic Performance

SELECT CONCAT(m.FirstName, ' ', m.LastName) AS Mechanic,
	AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate)) AS [Days going]
FROM Mechanics AS m
JOIN Jobs AS j ON
	m.MechanicId = j.MechanicId
GROUP BY m.FirstName, m.LastName, m.MechanicId
ORDER BY m.MechanicId


--8.	Available Mechanics
SELECT CONCAT(m.FirstName, ' ', m.LastName) AS Available
FROM Mechanics AS m
LEFT JOIN Jobs AS j ON
	m.MechanicId = j.MechanicId
WHERE 'Finished'= ALL
					(SELECT j.[Status] 
					FROM Jobs AS j
					WHERE j.MechanicId = m.MechanicId)
ORDER BY m.MechanicId

----------------------other solution-----------------------
SELECT CONCAT([FirstName], ' ' + [LastName]) AS [Available]
FROM [Mechanics]
WHERE [MechanicId] NOT IN
(
    SELECT DISTINCT
           m.[MechanicId]
    FROM [Mechanics] AS m
         JOIN [Jobs] AS j ON j.[MechanicId] = m.[MechanicId]
    WHERE j.[Status] = 'In Progress'
)
ORDER BY [MechanicId]

--9.	Past Expenses

SELECT j.[JobId],
		ISNULL(SUM(p.[Price] * op.[Quantity]),0.00) AS [Total]
FROM [Jobs] AS j
LEFT JOIN [Orders] AS o ON j.[JobId] = o.[JobId]
LEFT JOIN [OrderParts] AS op ON o.[OrderId] = op.[OrderId]
LEFT JOIN [Parts] AS p ON op.[PartId] = p.[PartId]
WHERE j.[Status] = 'Finished'
GROUP BY j.[JobId]
ORDER BY [Total] DESC,j.[JobId]

--10.	Missing Parts
SELECT 
	   P.PartId,
       P.Description,
       PN.Quantity as Required,
       P.StockQty,
	   IIF(O.Delivered = 0, OP.Quantity, 0)
FROM Parts AS P
	LEFT JOIN PartsNeeded AS PN ON P.PartId  = PN.PartId
	LEFT JOIN OrderParts  AS OP ON P.PartId  = OP.PartId
	LEFT JOIN Orders      AS O  ON O.OrderId = OP.OrderId
	LEFT JOIN Jobs        AS J  ON J.JobId   = PN.JobId
	WHERE J.Status !='Finished'  AND 
	(P.StockQty +  IIF(O.Delivered = 0, OP.Quantity, 0))< PN.Quantity
	ORDER BY PartId

--Section 4. Programmability
--11.	Place Order
GO
CREATE PROCEDURE usp_PlaceOrder(@JobId INT, @SerialNumber VARCHAR(MAX), @Quantity INT) 
AS
BEGIN
	DECLARE @JobStatus VARCHAR(MAX) = (SELECT [Status] FROM [Jobs] WHERE [JobId] = @JobId)
	DECLARE @JobExists BIT = (SELECT COUNT([JobId]) FROM [Jobs] WHERE [JobId] = @JobId)
	DECLARE @PartExists BIT = (SELECT COUNT([SerialNumber]) FROM [Parts] WHERE [SerialNumber] = @SerialNumber)

	IF(@Quantity <= 0)
		THROW 50012, 'Part quantity must be more than zero!' , 1

	IF(@JobStatus = 'Finished')
		THROW 50011, 'This job is not active!', 1

	IF(@JobExists = 0)
		THROW 50013, 'Job not found!', 1

	IF(@PartExists = 0)
		THROW 50014, 'Part not found!', 1

	DECLARE @OrderForJobExists INT = 
	(
		SELECT COUNT(o.[OrderId]) FROM [Orders] AS o
		WHERE o.[JobId] = @JobId AND o.[IssueDate] IS NULL
	)

	IF(@OrderForJobExists = 0)
	BEGIN
		INSERT INTO [Orders] VALUES
		(@JobId, NULL, 0)
	END

	DECLARE @OrderId INT = 
	(
		SELECT o.[OrderId] FROM [Orders] AS o
		WHERE o.[JobId] = @JobId AND o.[IssueDate] IS NULL
	)

	IF(@OrderId > 0 AND @PartExists = 1 AND @Quantity > 0)
	BEGIN
		DECLARE @PartId INT = (SELECT [PartId] FROM [Parts] WHERE [SerialNumber] = @SerialNumber)
		DECLARE @PartExistsInOrder INT = (SELECT COUNT(*) FROM [OrderParts] WHERE [OrderId] = @OrderId AND [PartId] = @PartId)

		IF(@PartExistsInOrder > 0)
		BEGIN
			UPDATE [OrderParts]
			SET [Quantity] += @Quantity
			WHERE [OrderId] = @OrderId AND [PartId] = @PartId
		END
		ELSE
		BEGIN
			INSERT INTO [OrderParts] VALUES
			(@OrderId, @PartId, @Quantity)
		END
	END
END

--Execution

DECLARE @err_msg AS NVARCHAR(MAX);
BEGIN TRY
  EXEC usp_PlaceOrder 1, 'ZeroQuantity', 0
END TRY

BEGIN CATCH
  SET @err_msg = ERROR_MESSAGE();
  SELECT @err_msg
END CATCH

--12.	Cost Of Order
GO
CREATE FUNCTION udf_GetCost (@jobID INT)
RETURNS DECIMAL(15, 2)
AS
BEGIN
	RETURN	
		ISNULL((SELECT SUM(p.Price * op.Quantity) AS Cost
		FROM Jobs AS j
		JOIN Orders AS o ON
			j.JobId = o.JobId
		JOIN OrderParts AS op ON
			o.OrderId = op.OrderId
		JOIN Parts AS p ON
			op.PartId = p.PartId
		GROUP BY j.JobId
		HAVING j.JobId = @jobID), 0)
END
GO
SELECT dbo.udf_GetCost(1)