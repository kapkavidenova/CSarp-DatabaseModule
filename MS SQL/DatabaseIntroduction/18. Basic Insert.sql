--18. Basic Insert
SELECT * FROM Towns
INSERT INTO Towns(Name)
	VALUES
		('Sofia'),
		('Plovdiv'),
		('Varna'),
		('Burgas')

		--SELECT * FROM Departments
INSERT INTO Departments([Name])
	VALUES
		('Engineering'),
		('Sales'),
		('Marketing'),
		('Software Development'),
		('Quality Assurance')

INSERT INTO Employees	 (FirstName,MiddleName,LastName,JobTitle,DepartmentId,HireDate,Salary)
	VALUES
		('Pesho' , 'P' , 'Petrov', 'Senior Engineer',1,'2010/05/06',3020.00),
('Merry' , 'P' , 'Ivanova', 'Quality Assurance',5,'2016/09/07',1000.00),
('Georgi' , 'I' , 'Ivanov', 'Sales',2,'2010/09/09',2000.00),
('Ivan' , 'I' , 'Ivanov', '.Marketing',3,'2010/10/10',1500)