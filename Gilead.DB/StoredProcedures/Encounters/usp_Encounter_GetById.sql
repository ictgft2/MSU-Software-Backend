CREATE OR ALTER PROCEDURE dbo.usp_Encounter_GetById @EncounterId uniqueidentifier AS
BEGIN
    SELECT * FROM dbo.Encounters WHERE Id = @EncounterId;
    SELECT p.* FROM dbo.Patients p JOIN dbo.Encounters e ON e.PatientId = p.Id WHERE e.Id = @EncounterId;
    SELECT * FROM dbo.VitalSigns WHERE EncounterId = @EncounterId ORDER BY RecordedAt DESC;
    SELECT * FROM dbo.ConsultationNotes WHERE EncounterId = @EncounterId;
    SELECT * FROM dbo.Prescriptions WHERE EncounterId = @EncounterId;
    SELECT * FROM dbo.LabRequests WHERE EncounterId = @EncounterId;
    SELECT r.* FROM dbo.LabResults r JOIN dbo.LabRequests q ON q.Id = r.LabRequestId WHERE q.EncounterId = @EncounterId;
    SELECT * FROM dbo.DressingOrders WHERE EncounterId = @EncounterId;
    SELECT * FROM dbo.ContactTraces WHERE EncounterId = @EncounterId;
END
