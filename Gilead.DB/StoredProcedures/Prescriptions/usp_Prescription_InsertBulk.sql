CREATE OR ALTER PROCEDURE dbo.usp_Prescription_InsertBulk @Prescriptions dbo.PrescriptionTvp READONLY AS
BEGIN
    INSERT dbo.Prescriptions (Id, ConsultationNoteId, EncounterId, DrugName, Dosage, Frequency, Duration, Route, Instructions, Status, IssuedAt)
    SELECT Id, ConsultationNoteId, EncounterId, DrugName, Dosage, Frequency, Duration, Route, Instructions, Status, IssuedAt FROM @Prescriptions;
END
