--23. Decrease Tax Rate
UPDATE Payments
SET
	TaxRate -= TaxRate * 0.03

SELECT TaxRate FROM Payments

--24. Delete All Records
TRUNCATE TABLE Occupancies