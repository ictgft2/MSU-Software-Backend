CREATE OR ALTER PROCEDURE dbo.usp_DrugHandover_GetById @Id uniqueidentifier AS
BEGIN
    SELECT * FROM dbo.DrugHandovers WHERE Id = @Id;
END
