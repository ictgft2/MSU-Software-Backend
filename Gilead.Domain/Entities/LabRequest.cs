using Gilead.Domain.Enums;

namespace Gilead.Domain.Entities;

public sealed class LabRequest
{
    public Guid Id { get; set; }
    public Guid ConsultationNoteId { get; set; }
    public Guid EncounterId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public string ClinicalIndication { get; set; } = string.Empty;
    public LabRequestStatus Status { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
}
