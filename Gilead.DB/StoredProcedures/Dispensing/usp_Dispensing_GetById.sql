CREATE OR ALTER PROCEDURE dbo.usp_Dispensing_GetById @Id uniqueidentifier AS
BEGIN
    SELECT * FROM dbo.Dispensings WHERE Id = @Id;
END
