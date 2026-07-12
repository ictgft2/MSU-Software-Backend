namespace Gilead.Domain.Entities;

public sealed class ContactTrace
{
    public Guid Id { get; set; }
    public Guid EncounterId { get; set; }
    public Guid RecordedBy { get; set; }
    public string NextOfKinName { get; set; } = string.Empty;
    public string NextOfKinPhone { get; set; } = string.Empty;
    public string NextOfKinRelationship { get; set; } = string.Empty;
    public string ResidentialAddress { get; set; } = string.Empty;
    public string WorkplaceAddress { get; set; } = string.Empty;
    public string DischargeNotes { get; set; } = string.Empty;
    public string? ReferralDestination { get; set; }
    public DateTimeOffset RecordedAt { get; set; }
}
