namespace MedicalUnitSystem.DTOs.Enums
{
    public enum PatientEnum
    {
        PatientId,
        Name,
        DateOfBirth,
        Email,
        Phone,
        DateCreated
    }
    public enum DoctorEnum
    {
        DoctorId,
        Name,
        Email,
        Phone,
        DateCreated
    }
    public enum LaboratoryTestEnum
    {
        LaboratoryTestId,
        LaboratoryTestTypeId,
        DateCreated
    }
    public enum LaboratoryTestTypeEnum
    {
        LaboratoryTestTypeId,
        LaboratoryTestTypeName,
        DateCreated
    }
    public enum ConsultationEnum
    {
        ConsultationId,
        ConsultationDate,
        FollowupDate,
        DateCreated
    }
    public enum PrescriptionEnum
    {
        PrescriptionId,
        ConsultationId,
        MedicationName,
        Dosage,
        Frequency,
        Instructions,
        IsDispensed,
        DateCreated
    }
    public enum WaitingQueueEnum
    {
         WaitingQueueId,
        PatientId,
        AttendedTo,
         DateQueued,
    }

    public enum VitalsEnum
    {
         PatientId,
        HeartRate,
        SystolicBloodPressure,
        DiastolicBloodPressure,
        Temperature,
        RespiratoryRate,
        OxygenSaturation,
         Notes,
       }
}
