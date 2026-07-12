CREATE OR ALTER PROCEDURE dbo.usp_ServiceWindow_Update @WindowId uniqueidentifier, @ColdCaseOpenTime time, @ColdCaseCloseTime time AS
BEGIN
    UPDATE dbo.ServiceTimeWindows SET ColdCaseOpenTime = @ColdCaseOpenTime, ColdCaseCloseTime = @ColdCaseCloseTime WHERE Id = @WindowId;
    SELECT * FROM dbo.ServiceTimeWindows WHERE Id = @WindowId;
END
