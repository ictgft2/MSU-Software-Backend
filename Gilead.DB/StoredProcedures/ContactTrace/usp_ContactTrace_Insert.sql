CREATE OR ALTER PROCEDURE dbo.usp_ContactTrace_Insert
    @Id uniqueidentifier, @EncounterId uniqueidentifier, @RecordedBy uniqueidentifier, @NextOfKinName nvarchar(200),
    @NextOfKinPhone nvarchar(40), @NextOfKinRelationship nvarchar(100), @ResidentialAddress nvarchar(500),
    @WorkplaceAddress nvarchar(500), @DischargeNotes nvarchar(max), @ReferralDestination nvarchar(250) = NULL, @RecordedAt datetimeoffset
AS
BEGIN
    INSERT dbo.ContactTraces VALUES (@Id, @EncounterId, @RecordedBy, @NextOfKinName, @NextOfKinPhone, @NextOfKinRelationship, @ResidentialAddress, @WorkplaceAddress, @DischargeNotes, @ReferralDestination, @RecordedAt);
    SELECT * FROM dbo.ContactTraces WHERE Id = @Id;
END
