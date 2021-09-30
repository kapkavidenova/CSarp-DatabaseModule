--09. Peaks in Rila
SELECT * FROM Mountains
SELECT MountainRange,PeakName,Elevation
	FROM Peaks AS p
JOIN Mountains AS m ON m.Id = p.MountainId
WHERE m.MountainRange = 'Rila'
ORDER BY p.Elevation DESC
