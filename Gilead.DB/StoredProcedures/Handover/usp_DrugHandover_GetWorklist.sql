CREATE OR ALTER PROCEDURE dbo.usp_DrugHandover_GetWorklist @Status nvarchar(30) = NULL AS
BEGIN
    SELECT * FROM dbo.DrugHandovers
    WHERE (@Status IS NULL OR (@Status = 'Pending' AND HandoverAt IS NULL) OR (@Status = 'Completed' AND HandoverAt IS NOT NULL))
    ORDER BY COALESCE(HandoverAt, '9999-12-31'), Id;
END
