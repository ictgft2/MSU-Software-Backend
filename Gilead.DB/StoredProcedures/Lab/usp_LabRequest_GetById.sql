CREATE OR ALTER PROCEDURE dbo.usp_LabRequest_GetById @RequestId uniqueidentifier AS
BEGIN
    SELECT * FROM dbo.LabRequests WHERE Id = @RequestId;
END
