CREATE DATABASE Movies

CREATE TABLE Directors(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	DirectorName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Genres(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	GenreName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX) 
)

CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	CategoryName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Movies(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	Title NVARCHAR(50) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id),
	CopyrightYear Date NOT NULL,
	[Length] DECIMAL(5,2),
	GenreId INT FOREIGN KEY REFERENCES Genres(Id),
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Rating DECIMAL(3,1),
	Notes NVARCHAR(MAX)
)

INSERT INTO Directors(DirectorName,Notes)
	VALUES
		('Bush',NULL),
		('Bush1',NULL),
		('Bush2',NULL),		
		('Bush3',NULL),
		('Bush4',NULL)


INSERT INTO Genres(GenreName,Notes)
	VALUES
		('f','To be checked'),
		('f','To be checked1'),
		('f','To be checked2'),
		('f','To be checked3'),
		('f','To be checked4')

INSERT INTO Categories(CategoryName,Notes)
	VALUES
		('Scientists',NULL),
		('Best Actor',NULL),
		('Best Music',NULL),
		('Best Design',NULL),
		('Dramatic performance',NULL)

INSERT INTO Movies(Title,DirectorId,CopyrightYear,[Length],GenreId,CategoryId,Rating,Notes)
	VALUES
		('Ocean',1,'2000',1.30,2,2,5,'Old one'),
		('Ocean',2,'2020',1.40,3,5,5,'New one'),
		('Ocean',3,'2010',1.50,5,4,5,NULL),
		('Ocean',4,'2011',1.55,1,1,5,'Like it!'),
		('Ocean',5,'2005',1.58,4,3,5,'Not bad')

		