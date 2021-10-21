--12. Calculating Interest
CREATE PROCEDURE usp_CalculateFutureValueForAccount(@accountId INT,
	@interestRate FLOAT)  
AS
	SELECT ah.Id AS [Account Id],
			ah.FirstName AS [First Name],
			ah.LastName AS [Last Name],
			a.Balance AS [Current Balance],
			(SELECT dbo.ufn_CalculateFutureValue(a.Balance,@interestRate,5)) AS [Balance in 5 years]
	
	FROM AccountHolders AS ah
	JOIN Accounts AS a ON ah.Id = a.AccountHolderId
	WHERE a.Id = @accountId
	GO
	
	EXEC usp_CalculateFutureValueForAccount 1,0.1

	USE master
