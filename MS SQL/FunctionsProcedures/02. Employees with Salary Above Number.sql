--02. Employees with Salary Above Number
CREATE PROCEDURE usp_GetEmployeesSalaryAboveNumber @Number DECIMAL(18,4)
AS
	SELECT FirstName AS [FirstName],LastName AS [LastName]
	FROM Employees
	WHERE Salary >= @Number
GO

EXEC  usp_GetEmployeesSalaryAboveNumber 48100