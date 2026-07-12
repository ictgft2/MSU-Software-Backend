CREATE OR ALTER PROCEDURE dbo.usp_ContactTrace_GetByEncounter @EncounterId uniqueidentifier AS
BEGIN
    SELECT * FROM dbo.ContactTraces WHERE EncounterId = @EncounterId;
END
