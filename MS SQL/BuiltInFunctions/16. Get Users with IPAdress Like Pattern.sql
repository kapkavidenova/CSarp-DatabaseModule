--16. Get Users with IPAdress Like Pattern
SELECT [Username],
	   [IpAddress] AS [IP Address]	 
FROM Users
WHERE [IpAddress] LIKE '___.1_%._%.___' 
ORDER BY Username