CREATE OR ALTER PROCEDURE dbo.usp_LabRequest_GetWorklist @Status nvarchar(30) = NULL, @Date date = NULL AS
BEGIN
    SELECT * FROM dbo.LabRequests
    WHERE (@Status IS NULL OR Status = @Status)
      AND (@Date IS NULL OR CAST(RequestedAt AS date) = @Date)
    ORDER BY RequestedAt;
END
