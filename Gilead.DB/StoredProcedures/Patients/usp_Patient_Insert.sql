CREATE OR ALTER PROCEDURE dbo.usp_Patient_Insert
    @Id uniqueidentifier, @FullName nvarchar(200), @Age int, @Sex nvarchar(1), @Phone nvarchar(40), @Address nvarchar(500),
    @NextOfKinName nvarchar(200), @NextOfKinPhone nvarchar(40), @NextOfKinRelationship nvarchar(100), @CreatedAt datetimeoffset
AS
BEGIN
    INSERT dbo.Patients VALUES (@Id, @FullName, @Age, @Sex, @Phone, @Address, @NextOfKinName, @NextOfKinPhone, @NextOfKinRelationship, @CreatedAt);
    SELECT * FROM dbo.Patients WHERE Id = @Id;
END
