CREATE OR ALTER PROCEDURE dbo.usp_LabResult_Insert
    @Id uniqueidentifier, @LabRequestId uniqueidentifier, @ScientistId uniqueidentifier, @TestName nvarchar(200),
    @Findings nvarchar(max), @Conclusion nvarchar(max), @Values nvarchar(max), @CompletedAt datetimeoffset
AS
BEGIN
    INSERT dbo.LabResults VALUES (@Id, @LabRequestId, @ScientistId, @TestName, @Findings, @Conclusion, @Values, @CompletedAt);
    UPDATE dbo.LabRequests SET Status = 'Completed' WHERE Id = @LabRequestId;
    SELECT * FROM dbo.LabResults WHERE Id = @Id;
END
