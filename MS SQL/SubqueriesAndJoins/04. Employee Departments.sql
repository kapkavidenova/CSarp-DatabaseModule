--04. Employee Departments
SELECT TOP(5) EmployeeID,FirstName,Salary,d.[Name] AS [DepartmentName] 
FROM Employees AS e
JOIN Departments AS d ON d.DepartmentID = e.DepartmentID
WHERE Salary > 15000
ORDER BY d.DepartmentID ASC