CREATE OR ALTER PROCEDURE dbo.usp_Prescription_AllHandedOverForEncounter @EncounterId uniqueidentifier AS
BEGIN
    SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM dbo.Prescriptions WHERE EncounterId = @EncounterId)
        AND NOT EXISTS (SELECT 1 FROM dbo.Prescriptions WHERE EncounterId = @EncounterId AND Status <> 'HandedOver')
        THEN 1 ELSE 0 END AS bit);
END
