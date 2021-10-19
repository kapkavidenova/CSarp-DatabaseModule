--05. Salary Level Function
GO
CREATE FUNCTION  ufn_GetSalaryLevel (@salary DECIMAL(18,4))
RETURNS NVARCHAR(7)
AS
BEGIN
	DECLARE @SalaryLevel NVARCHAR(7)

	SET @SalaryLevel=
		CASE
		WHEN @salary < 30000 THEN 'Low'
		WHEN @salary BETWEEN 30000 AND 50000 THEN 'Average'
		ELSE 'High'
	END

	RETURN @SalaryLevel
END
GO

SELECT Salary, (dbo.ufn_GetSalaryLevel(Salary)) AS [Salary Level]
FROM Employees
