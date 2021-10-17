--1. Employees with Salary Above 35000
GO
CREATE PROC dbo.usp_GetEmployeesSalaryAbove35000
AS
	SELECT FirstName, LastName
	FROM Employees
	WHERE Salary>35000
GO
EXEC usp_GetEmployeesSalaryAbove35000


