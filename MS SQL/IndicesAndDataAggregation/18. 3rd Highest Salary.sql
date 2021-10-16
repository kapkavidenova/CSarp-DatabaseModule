--18. *3rd Highest Salary
SELECT
	DISTINCT DepartmentID,Salary AS [ThirdHighestSalary]
FROM  
	(SELECT DepartmentID,Salary,
		DENSE_RANK() OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) AS SalaryRank
	 FROM Employees
	 ) AS dt
WHERE SalaryRank = 3