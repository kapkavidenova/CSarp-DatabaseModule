--11. Min Average Salary
SELECT MIN(avsal.AverageSalary) AS MinAverageSalary FROM
	(
		SELECT AVG(e.Salary) AS AverageSalary
		FROM Employees AS e
		GROUP BY e.DepartmentID
    ) AS avsal