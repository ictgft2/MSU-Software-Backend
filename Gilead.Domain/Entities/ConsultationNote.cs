namespace Gilead.Domain.Entities;

public sealed class ConsultationNote
{
    public Guid Id { get; set; }
    public Guid EncounterId { get; set; }
    public Guid DoctorId { get; set; }
    public string Diagnosis { get; set; } = "[]";
    public string ClinicalNotes { get; set; } = string.Empty;
    public bool RequiresLab { get; set; }
    public bool RequiresDressing { get; set; }
    public bool IsReferral { get; set; }
    public string? ReferralFacility { get; set; }
    public string? ReferralReason { get; set; }
    public DateTimeOffset ConsultedAt { get; set; }
}
