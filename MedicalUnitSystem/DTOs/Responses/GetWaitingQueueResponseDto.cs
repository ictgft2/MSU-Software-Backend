namespace MedicalUnitSystem.DTOs.Responses
{
    public class GetWaitingQueueResponseDto
    {
        public int WaitingQueueId { get; set; }
        public int PatientId { get; set; }
        public bool AttendedTo { get; set; }
        public DateTime DateQueued { get; set; }
    }
}
