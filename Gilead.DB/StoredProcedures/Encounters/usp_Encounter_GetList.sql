CREATE OR ALTER PROCEDURE dbo.usp_Encounter_GetList @Status nvarchar(40) = NULL, @Date date = NULL, @AdmissionType nvarchar(30) = NULL AS
BEGIN
    SELECT * FROM dbo.Encounters
    WHERE (@Status IS NULL OR Status = @Status)
      AND (@Date IS NULL OR CAST(CreatedAt AS date) = @Date)
      AND (@AdmissionType IS NULL OR AdmissionType = @AdmissionType)
    ORDER BY CreatedAt DESC;
END
