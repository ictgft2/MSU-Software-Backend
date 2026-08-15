CREATE OR ALTER PROCEDURE dbo.usp_VitalSigns_Insert
    @Id uniqueidentifier, @EncounterId uniqueidentifier, @RecordedBy uniqueidentifier, @BloodPressureSystolic int = NULL,
    @BloodPressureDiastolic int = NULL, @PulseRate int = NULL, @Temperature decimal(5,2) = NULL, @Spo2 int = NULL,
    @RespiratoryRate int = NULL, @Weight decimal(8,2) = NULL, @Notes nvarchar(1000) = NULL, @RecordedAt datetimeoffset
AS
BEGIN
    INSERT dbo.VitalSigns VALUES (@Id, @EncounterId, @RecordedBy, @BloodPressureSystolic, @BloodPressureDiastolic, @PulseRate, @Temperature, @Spo2, @RespiratoryRate, @Weight, @Notes, @RecordedAt);
    SELECT * FROM dbo.VitalSigns WHERE Id = @Id;
END
