--13. Scalar Function: Cash in User Games Odd Rows
CREATE FUNCTION ufn_CashInUsersGames (@gameName NVARCHAR(50))
RETURNS TABLE
AS
	RETURN SELECT SUM(Cash) AS SumCash
		FROM 
			(SELECT ROW_NUMBER() OVER (ORDER BY ug.Cash DESC) AS RowNumber,Cash
				FROM UsersGames AS ug
				JOIN Games AS g ON ug.GameId = g.Id
				 WHERE g.[Name] = @gameName
				 ) AS temp
				  WHERE RowNumber % 2 != 0
	
SELECT * FROM  dbo.ufn_CashInUsersGames('LOVE IN A MIST')
