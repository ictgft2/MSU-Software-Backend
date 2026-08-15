CREATE OR ALTER PROCEDURE dbo.usp_Dispensing_Insert
    @Id uniqueidentifier, @PrescriptionId uniqueidentifier, @PharmacistId uniqueidentifier, @DrugName nvarchar(200),
    @QuantityDispensed int, @BatchNumber nvarchar(100), @ExpiryDate date, @Notes nvarchar(1000) = NULL, @DispensedAt datetimeoffset
AS
BEGIN
    INSERT dbo.Dispensings VALUES (@Id, @PrescriptionId, @PharmacistId, @DrugName, @QuantityDispensed, @BatchNumber, @ExpiryDate, @Notes, @DispensedAt);
    SELECT * FROM dbo.Dispensings WHERE Id = @Id;
END
