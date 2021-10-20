--08. Delete Employees and Departments
CREATE PROCEDURE usp_DeleteEmployeesFromDepartment (@departmentId INT) 
AS
BEGIN
	DELETE
	FROM Employees
	WHERE EmployeeID IN
		( SELECT EmployeeID
		  FROM Employees
		  WHERE DepartmentID = @departmentId
	    )
	 
	 UPDATE Employees
	 SET ManagerID = NULL
	 WHERE ManagerID IN
		(SELECT EmployeeID
		 FROM Employees
		  WHERE DepartmentID = @departmentId)

	  ALTER TABLE Departments
				ALTER COLUMN ManagerID INT NULL;

	  UPDATE Departments
	  SET ManagerID = NULL
	  WHERE ManagerID IN
				( SELECT EmployeeID
				  FROM Employees
				  WHERE DepartmentID = @departmentId
				)

	  DELETE 
	  FROM Employees
	  WHERE DepartmentID = @departmentId

	  DELETE
	  FROM Departments
	  WHERE DepartmentID = @departmentId

	  SELECT COUNT(*)
	  FROM Employees
	  WHERE DepartmentID = @departmentId


END