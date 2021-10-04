--14. Games from 2011 and 2012 year
SELECT TOP(50) [Name],
	FORMAT([Start],'yyyy-MM-dd') AS [Start]
FROM Games AS g
WHERE DATEPART(YEAR,[Start]) IN (2011,2012)
ORDER BY g.[Start] ASC, [Name]