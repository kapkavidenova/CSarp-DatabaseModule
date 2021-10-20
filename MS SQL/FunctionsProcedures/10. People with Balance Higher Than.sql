--10. People with Balance Higher Than
CREATE PROCEDURE usp_GetHoldersWithBalanceHigherThan (@num DECIMAL(18,4))
AS
BEGIN
	
	SELECT ah.FirstName, ah.LastName
	FROM AccountHolders AS ah
	JOIN Accounts AS a ON ah.Id = a.AccountHolderId
	GROUP BY ah.FirstName,ah.LastName
	HAVING SUM(a.Balance) > @num
	ORDER BY ah.FirstName,ah.LastName

END

EXEC usp_GetHoldersWithBalanceHigherThan 10000

