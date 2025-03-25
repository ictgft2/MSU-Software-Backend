namespace MedicalUnitSystem.DTOs.Requests
{
    public class UpdateWaitingQueueRequestDto
    {
        public int PatientId { get; set; }
        public bool AttendedTo { get; set; }
    }
}
