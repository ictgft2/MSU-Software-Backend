CREATE OR ALTER PROCEDURE dbo.usp_Register_ExportDrugs @Date date = NULL AS
BEGIN
    SELECT * FROM dbo.vw_DrugRegister
    WHERE (@Date IS NULL OR CAST(HandoverAt AS date) = @Date)
    ORDER BY HandoverAt DESC;
END
