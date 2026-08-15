CREATE OR ALTER PROCEDURE dbo.usp_Register_GetDrugs @Date date = NULL, @Page int = 1, @Limit int = 50 AS
BEGIN
    SELECT * FROM dbo.vw_DrugRegister
    WHERE (@Date IS NULL OR CAST(HandoverAt AS date) = @Date)
    ORDER BY HandoverAt DESC
    OFFSET (@Page - 1) * @Limit ROWS FETCH NEXT @Limit ROWS ONLY;
END
