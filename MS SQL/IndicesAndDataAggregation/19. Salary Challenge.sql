--19. Salary Challenge
SELECT 
	e.FirstName, e.LastName, e.DepartmentID
FROM Employees AS e

JOIN (
		SELECT DepartmentID,AVG(Salary) AS AvgSalary
		FROM Employees
		GROUP BY DepartmentID
	 ) AS j ON e.DepartmentID = j.DepartmentID

WHERE Salary>AvgSalary
ORDER BY e.DepartmentID

