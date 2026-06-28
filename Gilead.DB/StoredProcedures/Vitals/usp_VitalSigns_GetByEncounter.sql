CREATE OR ALTER PROCEDURE dbo.usp_VitalSigns_GetByEncounter @EncounterId uniqueidentifier AS
BEGIN
    SELECT * FROM dbo.VitalSigns WHERE EncounterId = @EncounterId ORDER BY RecordedAt DESC;
END
