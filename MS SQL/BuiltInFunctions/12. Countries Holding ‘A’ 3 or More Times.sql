--12. Countries Holding ‘A’ 3 or More Times
SELECT CountryName AS [Country Name],IsoCode 
FROM Countries
WHERE LEN(CountryName) - LEN(Replace(CountryName,'A',''))>=3
ORDER BY IsoCode