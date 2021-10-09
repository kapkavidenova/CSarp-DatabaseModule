--15. *Continents and Currencies
SELECT [ContinentCode],
		[CurrencyCode],
		[CurrencyCount] AS [CurrencyUsage]
FROM (SELECT *,
	DENSE_RANK() OVER(PARTITION BY ContinentCode 
		ORDER BY CurrencyCount DESC) AS [CurrencyRank]
	FROM (
				SELECT c.ContinentCode,c.CurrencyCode,COUNT(c.CurrencyCode) AS [CurrencyCount]
				FROM Countries AS c
				GROUP BY c.ContinentCode,c.CurrencyCode
          ) AS [CurrencyCountSubQuery]
		WHERE CurrencyCount > 1
     ) AS [CurrencyRankingSubQuery]
WHERE [CurrencyRank] = 1
ORDER BY [ContinentCode]