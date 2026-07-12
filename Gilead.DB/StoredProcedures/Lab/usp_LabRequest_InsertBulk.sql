CREATE OR ALTER PROCEDURE dbo.usp_LabRequest_InsertBulk @LabRequests dbo.LabRequestTvp READONLY AS
BEGIN
    INSERT dbo.LabRequests (Id, ConsultationNoteId, EncounterId, TestName, ClinicalIndication, Status, RequestedAt)
    SELECT Id, ConsultationNoteId, EncounterId, TestName, ClinicalIndication, Status, RequestedAt FROM @LabRequests;
END
