--03. Town Names Starting With
GO
CREATE PROCEDURE usp_GetTownsStartingWith @Model NVARCHAR(50)
AS
	SELECT [Name] AS Town
	FROM Towns AS t WHERE t.[Name] LIKE @Model+'%'
GO
EXEC usp_GetTownsStartingWith b