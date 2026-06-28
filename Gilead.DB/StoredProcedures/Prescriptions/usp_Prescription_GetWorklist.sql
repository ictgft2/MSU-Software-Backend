CREATE OR ALTER PROCEDURE dbo.usp_Prescription_GetWorklist @Status nvarchar(30) = NULL, @Date date = NULL AS
BEGIN
    SELECT * FROM dbo.Prescriptions
    WHERE (@Status IS NULL OR Status = @Status)
      AND (@Date IS NULL OR CAST(IssuedAt AS date) = @Date)
    ORDER BY IssuedAt;
END
