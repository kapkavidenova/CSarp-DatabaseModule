--06. Find Towns Starting With
 SELECT TownID,[Name]	
	FROM Towns
	WHERE [Name] LIKE '[M,K,B,E]%'
	ORDER BY [Name] ASC