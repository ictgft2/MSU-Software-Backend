CREATE OR ALTER PROCEDURE dbo.usp_LabResult_GetByEncounter @EncounterId uniqueidentifier AS
BEGIN
    SELECT r.* FROM dbo.LabResults r JOIN dbo.LabRequests q ON q.Id = r.LabRequestId WHERE q.EncounterId = @EncounterId ORDER BY r.CompletedAt DESC;
END
