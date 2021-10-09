--13. Count Mountain Ranges
SELECT c.CountryCode,
		COUNT(mc.MountainId)  AS [MountainRanges]
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
LEFT JOIN Mountains AS m ON mc.MountainId = m.Id
WHERE c.CountryCode IN ('BG','RU','US')
GROUP BY c.CountryCode