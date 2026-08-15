CREATE OR ALTER PROCEDURE dbo.usp_Consultation_GetByEncounter @EncounterId uniqueidentifier AS
BEGIN
    SELECT * FROM dbo.ConsultationNotes WHERE EncounterId = @EncounterId;
END
