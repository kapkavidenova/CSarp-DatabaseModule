--07. Find Towns Not Starting With
SELECT TownID,[Name]
	FROM Towns
	WHERE [Name] NOT LIKE '[R,B,D]%'
	ORDER BY [Name] ASC