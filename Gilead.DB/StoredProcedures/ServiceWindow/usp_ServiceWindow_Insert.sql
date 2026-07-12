CREATE OR ALTER PROCEDURE dbo.usp_ServiceWindow_Insert
    @Id uniqueidentifier, @Date date, @ColdCaseOpenTime time, @ColdCaseCloseTime time, @CreatedBy uniqueidentifier, @CreatedAt datetimeoffset
AS
BEGIN
    INSERT dbo.ServiceTimeWindows VALUES (@Id, @Date, @ColdCaseOpenTime, @ColdCaseCloseTime, @CreatedBy, @CreatedAt);
    SELECT * FROM dbo.ServiceTimeWindows WHERE Id = @Id;
END
