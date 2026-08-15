CREATE OR ALTER PROCEDURE dbo.usp_ContactTrace_Update
    @Id uniqueidentifier, @EncounterId uniqueidentifier, @RecordedBy uniqueidentifier, @NextOfKinName nvarchar(200),
    @NextOfKinPhone nvarchar(40), @NextOfKinRelationship nvarchar(100), @ResidentialAddress nvarchar(500),
    @WorkplaceAddress nvarchar(500), @DischargeNotes nvarchar(max), @ReferralDestination nvarchar(250) = NULL, @RecordedAt datetimeoffset
AS
BEGIN
    UPDATE dbo.ContactTraces SET RecordedBy = @RecordedBy, NextOfKinName = @NextOfKinName, NextOfKinPhone = @NextOfKinPhone,
        NextOfKinRelationship = @NextOfKinRelationship, ResidentialAddress = @ResidentialAddress, WorkplaceAddress = @WorkplaceAddress,
        DischargeNotes = @DischargeNotes, ReferralDestination = @ReferralDestination, RecordedAt = @RecordedAt
    WHERE Id = @Id AND EncounterId = @EncounterId;
    SELECT * FROM dbo.ContactTraces WHERE Id = @Id;
END
