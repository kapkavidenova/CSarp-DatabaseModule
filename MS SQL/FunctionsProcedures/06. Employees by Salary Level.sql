--06. Employees by Salary Level
CREATE PROCEDURE usp_EmployeesBySalaryLevel @LevelOfSalary NVARCHAR(7)
AS
	SELECT FirstName, LastName
	FROM Employees

	WHERE dbo.ufn_GetSalaryLevel(Salary) = @LevelOfSalary
GO

EXEC usp_EmployeesBySalaryLevel 'High'
