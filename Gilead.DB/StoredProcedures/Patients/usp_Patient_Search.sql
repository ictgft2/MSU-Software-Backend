CREATE OR ALTER PROCEDURE dbo.usp_Patient_Search @Name nvarchar(200) = NULL, @Phone nvarchar(40) = NULL AS
BEGIN
    SELECT * FROM dbo.Patients
    WHERE (@Name IS NULL OR FullName LIKE '%' + @Name + '%')
      AND (@Phone IS NULL OR Phone = @Phone)
    ORDER BY CreatedAt DESC;
END
