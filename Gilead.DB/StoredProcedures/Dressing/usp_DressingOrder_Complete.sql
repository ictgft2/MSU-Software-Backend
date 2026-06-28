CREATE OR ALTER PROCEDURE dbo.usp_DressingOrder_Complete @OrderId uniqueidentifier, @PerformedBy uniqueidentifier, @ProcedureNotes nvarchar(max) = NULL AS
BEGIN
    UPDATE dbo.DressingOrders SET Status = 'Completed', PerformedBy = @PerformedBy, ProcedureNotes = @ProcedureNotes, CompletedAt = SYSDATETIMEOFFSET() WHERE Id = @OrderId;
END
