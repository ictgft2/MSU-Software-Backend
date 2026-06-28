CREATE OR ALTER PROCEDURE dbo.usp_ServiceWindow_GetCurrent @Date date AS
BEGIN
    SELECT * FROM dbo.ServiceTimeWindows WHERE [Date] = @Date;
END
