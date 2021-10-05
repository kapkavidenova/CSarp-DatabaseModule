--17. Show All Games with Duration and Part of the Day
SELECT 
	[Name],
	CASE
		WHEN (DATEPART(hour,[Start])<12) THEN 'Morning'
		WHEN (DATEPART(hour,[Start])<18) THEN 'Afternoon'
		ELSE 'Evening'
	END AS [Part of the Day],

	CASE	
		WHEN (Duration<=3) THEN 'Extra Short'
		WHEN (Duration<=6) THEN 'Short'
		WHEN (Duration>6) THEN 'Long'
		ELSE 'Extra Long'
	END AS [Duration]
FROM Games
ORDER BY [Name] ASC, [Duration] ASC

