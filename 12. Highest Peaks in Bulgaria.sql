--12. Highest Peaks in Bulgaria
SELECT c.CountryCode,
		m.MountainRange,
		p.PeakName,
		p.Elevation
FROM Countries AS c

LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
LEFT JOIN Mountains AS m ON mc.MountainId = m.Id
LEFT JOIN Peaks AS p ON m.Id = p.MountainId
WHERE p.Elevation>2835 AND c.CountryCode = 'BG'
ORDER BY p.Elevation DESC 