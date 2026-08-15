namespace Gilead.Domain.Entities;

public sealed class DrugHandover
{
    public Guid Id { get; set; }
    public Guid DispensingId { get; set; }
    public Guid EncounterId { get; set; }
    public Guid? ProtocolOfficerId { get; set; }
    public bool PatientNameVerified { get; set; }
    public bool DrugListVerified { get; set; }
    public bool DosageCounsellingDone { get; set; }
    public bool DurationCounsellingDone { get; set; }
    public string? CounsellingNotes { get; set; }
    public DateTimeOffset? HandoverAt { get; set; }
}
