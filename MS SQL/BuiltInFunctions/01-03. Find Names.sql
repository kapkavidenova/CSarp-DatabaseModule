--01. Find Names of All Employees by First Name
USE SOFTUNI
SELECT FirstName, LastName 
	FROM Employees
	WHERE LEFT(FirstName,2) = 'Sa'


--02. Find Names of All employees by Last Name 
SELECT FirstName, LastName
	FROM Employees
	WHERE LastName LIKE '%ei%'


--03. Find First Names of All Employees
SELECT FirstName 
	FROM Employees
	WHERE DepartmentID IN (3,10)AND DATEPART(YEAR,HireDate)>=1995 AND DATEPART(YEAR,HireDate)<=2005 