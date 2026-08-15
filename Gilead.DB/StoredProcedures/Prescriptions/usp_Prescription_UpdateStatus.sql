CREATE OR ALTER PROCEDURE dbo.usp_Prescription_UpdateStatus @Id uniqueidentifier, @Status nvarchar(30) AS
BEGIN
    UPDATE dbo.Prescriptions SET Status = @Status WHERE Id = @Id;
END
