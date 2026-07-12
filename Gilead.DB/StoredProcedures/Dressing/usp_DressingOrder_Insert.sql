CREATE OR ALTER PROCEDURE dbo.usp_DressingOrder_Insert
    @Id uniqueidentifier, @ConsultationNoteId uniqueidentifier, @EncounterId uniqueidentifier, @Instructions nvarchar(max),
    @Status nvarchar(30), @PerformedBy uniqueidentifier = NULL, @ProcedureNotes nvarchar(max) = NULL, @CompletedAt datetimeoffset = NULL,
    @CreatedAt datetimeoffset
AS
BEGIN
    INSERT dbo.DressingOrders VALUES (@Id, @ConsultationNoteId, @EncounterId, @Instructions, @Status, @PerformedBy, @ProcedureNotes, @CompletedAt, @CreatedAt);
END
