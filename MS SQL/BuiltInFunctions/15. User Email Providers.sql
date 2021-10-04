--15. User Email Providers
SELECT 
		Username,
	     RIGHT(Email,LEN(Email) - CHARINDEX('@',Email)) AS [Email Provider]
	FROM Users
	ORDER BY [Email Provider] ASC,[Username] ASC

--SELECT 
----[Email],
--	Username, SUBSTRING(Email,CHARINDEX('@',Email)+1,LEN(Email)) 
--	AS [Imail Provider]
--FROM Users
--	ORDER BY [Imail Provider],Username

