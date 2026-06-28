CREATE OR ALTER PROCEDURE dbo.usp_Patient_GetById @PatientId uniqueidentifier AS
BEGIN
    SELECT * FROM dbo.Patients WHERE Id = @PatientId;
END
