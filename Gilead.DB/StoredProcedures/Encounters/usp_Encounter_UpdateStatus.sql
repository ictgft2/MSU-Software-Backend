CREATE OR ALTER PROCEDURE dbo.usp_Encounter_UpdateStatus @EncounterId uniqueidentifier, @Status nvarchar(40), @DischargedAt datetimeoffset = NULL AS
BEGIN
    UPDATE dbo.Encounters SET Status = @Status, DischargedAt = COALESCE(@DischargedAt, DischargedAt), UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @EncounterId;
END
