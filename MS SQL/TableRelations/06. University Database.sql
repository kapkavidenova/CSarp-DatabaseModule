
--06. University Database
CREATE DATABASE University

CREATE TABLE Subjects(
	SubjectID INT PRIMARY KEY,
	SubjectName NVARCHAR(50)
)

CREATE TABLE Majors(
	MajorID INT PRIMARY KEY,
	[Name] NVARCHAR(50)
)

CREATE TABLE Students(
	StudentID INT PRIMARY KEY,
	StudentNumber CHAR(8),
	StudentName NVARCHAR(50),
	MajorID INT NOT NULL FOREIGN KEY REFERENCES Majors(MajorID)
)

CREATE TABLE Payments(
	PaymentID INT PRIMARY KEY,
	PaymentDate Date NOT NULL,
	PaymentAmount DECIMAL(6,2),
	StudentID INT NOT NULL FOREIGN KEY REFERENCES Students(StudentID)
)

CREATE TABLE Agenda(
	StudentID INT NOT NULL FOREIGN KEY REFERENCES Students(StudentID),
	SubjectID INT NOT NULL FOREIGN KEY REFERENCES Subjects(SubjectID),
	PRIMARY KEY(SubjectID,StudentID)

)
