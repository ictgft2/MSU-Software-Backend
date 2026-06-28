CREATE OR ALTER PROCEDURE dbo.usp_VitalSigns_GetLatest @EncounterId uniqueidentifier AS
BEGIN
    SELECT TOP 1 * FROM dbo.VitalSigns WHERE EncounterId = @EncounterId ORDER BY RecordedAt DESC;
END
