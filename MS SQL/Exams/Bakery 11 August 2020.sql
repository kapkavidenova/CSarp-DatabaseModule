GO
CREATE TABLE Countries(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] NVARCHAR(50) UNIQUE NOT NULL,
)

CREATE TABLE Products(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] NVARCHAR(25) UNIQUE NOT NULL,
	[Description] NVARCHAR(250) NOT NULL,
	Recipe NVARCHAR(MAX) NOT NULL,
	Price DECIMAL(18,2) NOT NULL,
	CHECK(Price>=0) 
)

CREATE TABLE Customers(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	FirstName NVARCHAR(25) NOT NULL,
	LastName NVARCHAR(25) NOT NULL,
	Gender CHAR(1) NOT NULL,
	CHECK(Gender='M' OR Gender='F'),
	Age INT NOT NULL,
	PhoneNumber CHAR(10) NOT NULL,
	CountryId INT FOREIGN KEY REFERENCES Countries(Id) 

)

CREATE TABLE Feedbacks(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	[Description] NVARCHAR(255),
	Rate DECIMAL(10,2) NOT NULL,
	CHECK(Rate>=0 AND Rate<=10),
	ProductId INT FOREIGN KEY REFERENCES Products(Id) NOT NULL,
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id) NOT NULL

)

CREATE TABLE Distributors(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] NVARCHAR(25) UNIQUE NOT NULL,
	AddressText NVARCHAR(30) NOT NULL,
	Summary NVARCHAR(200) NOT NULL,
	CountryId INT FOREIGN KEY REFERENCES Countries(Id) NOT NULL

)

CREATE TABLE Ingredients(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] NVARCHAR(30) NOT NULL,
	Description NVARCHAR(200) NOT NULL,
	OriginCountryId INT FOREIGN KEY REFERENCES Countries(Id) NOT NULL,
	DistributorId INT FOREIGN KEY REFERENCES Distributors(Id) NOT NULL

)

CREATE TABLE ProductsIngredients(
	ProductId INT FOREIGN KEY REFERENCES Products(Id) NOT NULL,
	IngredientId INT FOREIGN KEY REFERENCES Ingredients(Id) NOT NULL,
	PRIMARY KEY(ProductId,IngredientId)

)

--Section 2. DML 
--2.	Insert
INSERT INTO Distributors
		VALUES
			('Deloitte & Touche',2,	'6 Arch St #9757','Customizable neutral traveling'),
			('Congress Title',13,	'58 Hancock St','Customer loyalty'),
			('Kitchen People',1,'3 E 31st St #77','Triple-buffered stable delivery'),
			('General Color Co Inc',21,	'6185 Bohn St #72','Focus group'),
			('Beck Corporation',23,	'21 E 64th Ave','Quality-focused 4th generation hardware')


			INSERT INTO Customers
VALUES
('Francoise', 'Rautenstrauch', 15, 'M', '0195698399', 5),
('Kendra', 'Loud', 22, 'F', '0063631526', 11),
('Lourdes', 'Bauswell', 50, 'M', '0139037043', 8),
('Hannah', 'Edmison', 18, 'F', '0043343686', 1),
('Tom', 'Loeza', 31, 'M', '0144876096', 23),
('Queenie', 'Kramarczyk', 30, 'F', '0064215793', 29),
('Hiu', 'Portaro', 25, 'M', '0068277755', 16),
('Josefa', 'Opitz', 43, 'F', '0197887645', 17)
SELECT * FROM Customers


--3.	Update
SELECT *
FROM Ingredients
UPDATE Ingredients
	SET DistributorId = 35
	WHERE [Name] = 'Bay Leaf'OR  [Name]='Paprika' OR [Name]='Poppy'

UPDATE Ingredients
	SET OriginCountryId = 14
	WHERE OriginCountryId = 8


--4.	Delete
SELECT *
FROM Feedbacks
DELETE Feedbacks
WHERE CustomerId = 14 OR ProductId = 5

---Section 3. Querying 
--5.	Products by Price
SELECT [Name],
		Price,
		[Description]
FROM Products
ORDER BY Price DESC,[Name] ASC

--6.	Negative Feedback
SELECT f.ProductId,
		f.Rate,
		f.[Description],
		f.CustomerId,
		c.Age,
		c.Gender
		
FROM Feedbacks AS f
LEFT JOIN Customers AS c ON f.CustomerId = c.Id
WHERE f.Rate<5.0
ORDER BY ProductId DESC,f.Rate ASC

--7.	Customers without Feedback
SELECT CONCAT(c.FirstName,' ',c.LastName) AS [CustomerName],
		c.PhoneNumber,
		c.Gender		
FROM Customers AS c

LEFT JOIN Feedbacks AS f ON c.Id = f.CustomerId
WHERE f.CustomerId IS NULL
ORDER BY c.Id ASC

--8.	Customers by Criteria
SELECT c.FirstName,
		c.Age,
		c.PhoneNumber
FROM Customers AS c
JOIN Countries AS ct ON c.CountryId = ct.Id 
WHERE (Age>=21 AND FirstName LIKE '%an%') OR (PhoneNumber LIKE '%38' AND ct.[Name] != 'Greece')
ORDER BY c.FirstName ASC,c.Age DESC

--9.	Middle Range Distributors
SELECT d.[Name] AS [DistributorName],
		i.[Name] AS [IngredientName],
		p.[Name] AS [ProductName],
		AVG(f.Rate) AS [AverageRate]
FROM Distributors AS d
LEFT JOIN Ingredients AS i ON d.Id = i.DistributorId
LEFT JOIN ProductsIngredients AS pri ON i.Id = pri.IngredientId
LEFT JOIN Products AS p ON pri.ProductId = p.Id
LEFT JOIN Feedbacks AS f ON p.Id = f.ProductId
GROUP BY d.[Name],i.[Name],p.[Name]
HAVING AVG(f.Rate) BETWEEN 5 AND 8
ORDER BY[DistributorName],[IngredientName],[ProductName]

--10.	Country Representative
SELECT
	CountryName,
	DistributorName

FROM (SELECT c.[Name] AS CountryName,
		d.[Name] AS DistributorName,
		COUNT(i.Id) AS Count,
		DENSE_RANK() OVER (PARTITION BY c.Name ORDER BY COUNT(i.Id) DESC) AS [Rank]
		FROM Countries AS c
		LEFT JOIN Distributors AS d ON c.Id = d.CountryId
		LEFT JOIN Ingredients AS i ON d.Id = i.DistributorId
		GROUP BY c.[Name],d.[Name]
	 ) AS [TempSubQuery]
WHERE Rank = 1
ORDER BY CountryName,DistributorName

--Section 4. Programmability 
--11.	Customers with Countries
CREATE VIEW v_UserWithCountries AS
SELECT CONCAT(cus.FirstName,' ',cus.LastName) AS [CustomerName],
		cus.Age,
		cus.Gender,
		c.[Name]
FROM Customers AS cus
LEFT JOIN Countries AS c ON cus.CountryId = c.Id

SELECT TOP 5 *
  FROM v_UserWithCountries
 ORDER BY Age


 --12.	Delete Products
 GO
 CREATE TRIGGER DeleteProducts
 ON Products
 INSTEAD OF DELETE
 AS 
 BEGIN
 DELETE
	FROM Feedbacks
	WHERE ProductId IN
	(SELECT p.Id FROM Products AS p
	JOIN deleted AS d ON p.Id = d.Id)

 DELETE 
	 FROM ProductsIngredients
	 WHERE ProductId IN
	 (SELECT p.Id FROM Products AS p
	 JOIN deleted AS d ON p.Id = d.Id)

DELETE 
	FROM Products
	WHERE Products.Id IN
	(SELECT p.Id FROM Products AS p
	JOIN deleted AS d ON p.Id = d.Id)
END




