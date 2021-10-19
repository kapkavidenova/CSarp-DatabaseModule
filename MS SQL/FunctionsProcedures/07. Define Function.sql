--07. Define Function
GO
CREATE FUNCTION ufn_IsWordComprised(@setOfLetters NVARCHAR(50), @word NVARCHAR(50)) 
RETURNS BIT
AS
BEGIN
	DECLARE @index INT = 1;
	WHILE (@index <= LEN(@word))
		BEGIN
			IF CHARINDEX(SUBSTRING(@word,@index,1),@setOfLetters) = 0
				BEGIN
					RETURN 0
				END
				SET @index += 1
		END
RETURN 1
END
GO
SELECT [dbo.ufn__IsWordComprised]('oistmiahf', 'Sofia')