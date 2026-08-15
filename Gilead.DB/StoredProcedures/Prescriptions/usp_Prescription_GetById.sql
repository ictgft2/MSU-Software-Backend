CREATE OR ALTER PROCEDURE dbo.usp_Prescription_GetById @Id uniqueidentifier AS
BEGIN
    SELECT * FROM dbo.Prescriptions WHERE Id = @Id;
END
