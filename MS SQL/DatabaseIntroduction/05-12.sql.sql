--05. Truncate Table Minions
TRUNCATE TABLE Minions

--06. Drop All Tables
DROP TABLE Minions
DROP TABLE Towns

--07. Create Table People
CREATE TABLE People(
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	[Name] NVARCHAR(200) NOT NULL,
	Picture VARBINARY(MAX)
	CHECK(DATALENGTH(Picture) <= 900*1024), 
	Height DECIMAL(MAX,2),
)

--08. Create Table Users
CREATE TABLE Users(
	Id BIGINT PRIMARY KEY IDENTITY NOT NULL,
	Username VARCHAR(30) UNIQUE NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	ProfilePicture VARBINARY(MAX)
	CHECK (DATALENGTH(ProfilePicture)<= 900* 1024),
	LastLoginTime DATETIME,
	IsDeleted BIT NOT NULL
)

INSERT INTO Users(Username,Password, ProfilePicture,LastLoginTime,IsDeleted)
	VALUES
		('Niki',123,111,'08-26-2011 ',0),
		('Niki1',124,111,'08-26-2012 ',0),
		('Niki2',125,111,'08-26-2013 ',0),
		('Niki3',126,111,'08-26-2014 ',0),
		('Niki4',127,111,'08-26-2015 ',0)

SELECT * FROM Users

--09. Change Primary Key
ALTER TABLE Users
DROP CONSTRAINT [PK_Users]

ALTER TABLE Users
ADD CONSTRAINT PK_Users_CompositeIdUsername
PRIMARY KEY (Id, Username)


--10. Add Check Constraint
ALTER TABLE Users
ADD CONSTRAINT CK_Users_PasswordLength
CHECK (LEN([Password])>=3)


--11.	Set Default Value of a Field
ALTER TABLE Users
	ADD CONSTRAINT DF_Users_LastLoginTime
	DEFAULT GETDATE() FOR LastLoginTime


--12. Set Unique Field
ALTER TABLE Users
DROP CONSTRAINT PK_Users_CompositeIdUsername

ALTER TABLE Users
ADD CONSTRAINT PK_Users_IdPrimaryKey
PRIMARY KEY (Id)

ALTER TABLE Users
ADD CONSTRAINT CK_Users_Length
CHECK (LEN(Username)>=2)

