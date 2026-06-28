CREATE OR ALTER PROCEDURE dbo.usp_DressingOrder_GetById @OrderId uniqueidentifier AS
BEGIN
    SELECT * FROM dbo.DressingOrders WHERE Id = @OrderId;
END
