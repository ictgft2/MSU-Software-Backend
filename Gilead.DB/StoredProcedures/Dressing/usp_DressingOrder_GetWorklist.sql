CREATE OR ALTER PROCEDURE dbo.usp_DressingOrder_GetWorklist @Status nvarchar(30) = NULL AS
BEGIN
    SELECT * FROM dbo.DressingOrders WHERE (@Status IS NULL OR Status = @Status) ORDER BY CreatedAt;
END
