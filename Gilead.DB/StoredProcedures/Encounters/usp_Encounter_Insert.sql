CREATE OR ALTER PROCEDURE dbo.usp_Encounter_Insert
    @Id uniqueidentifier, @PatientId uniqueidentifier, @AdmissionType nvarchar(30), @Status nvarchar(40), @ArrivalMode nvarchar(30),
    @ChiefComplaint nvarchar(1000), @RegisteredBy uniqueidentifier, @AdmittedAt datetimeoffset, @DischargedAt datetimeoffset = NULL,
    @CreatedAt datetimeoffset, @UpdatedAt datetimeoffset
AS
BEGIN
    INSERT dbo.Encounters VALUES (@Id, @PatientId, @AdmissionType, @Status, @ArrivalMode, @ChiefComplaint, @RegisteredBy, @AdmittedAt, @DischargedAt, @CreatedAt, @UpdatedAt);
    SELECT * FROM dbo.Encounters WHERE Id = @Id;
END
