using Gilead.Domain.Enums;

namespace Gilead.Domain.Entities;

public sealed class Prescription
{
    public Guid Id { get; set; }
    public Guid ConsultationNoteId { get; set; }
    public Guid EncounterId { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public DrugRoute Route { get; set; }
    public string? Instructions { get; set; }
    public PrescriptionStatus Status { get; set; }
    public DateTimeOffset IssuedAt { get; set; }
}
