CREATE OR ALTER PROCEDURE dbo.usp_DrugHandover_Insert
    @Id uniqueidentifier, @DispensingId uniqueidentifier, @EncounterId uniqueidentifier, @ProtocolOfficerId uniqueidentifier = NULL,
    @PatientNameVerified bit = 0, @DrugListVerified bit = 0, @DosageCounsellingDone bit = 0, @DurationCounsellingDone bit = 0,
    @CounsellingNotes nvarchar(1000) = NULL, @HandoverAt datetimeoffset = NULL
AS
BEGIN
    INSERT dbo.DrugHandovers VALUES (@Id, @DispensingId, @EncounterId, @ProtocolOfficerId, @PatientNameVerified, @DrugListVerified, @DosageCounsellingDone, @DurationCounsellingDone, @CounsellingNotes, @HandoverAt);
    SELECT * FROM dbo.DrugHandovers WHERE Id = @Id;
END
