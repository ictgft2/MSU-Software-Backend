CREATE OR ALTER PROCEDURE dbo.usp_DrugHandover_Confirm
    @Id uniqueidentifier, @DispensingId uniqueidentifier, @EncounterId uniqueidentifier, @ProtocolOfficerId uniqueidentifier,
    @PatientNameVerified bit, @DrugListVerified bit, @DosageCounsellingDone bit, @DurationCounsellingDone bit,
    @CounsellingNotes nvarchar(1000) = NULL, @HandoverAt datetimeoffset
AS
BEGIN
    UPDATE dbo.DrugHandovers
    SET ProtocolOfficerId = @ProtocolOfficerId, PatientNameVerified = @PatientNameVerified, DrugListVerified = @DrugListVerified,
        DosageCounsellingDone = @DosageCounsellingDone, DurationCounsellingDone = @DurationCounsellingDone,
        CounsellingNotes = @CounsellingNotes, HandoverAt = @HandoverAt
    WHERE Id = @Id;
END
