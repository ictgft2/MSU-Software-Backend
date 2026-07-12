using Gilead.Domain.Enums;

namespace Gilead.Domain.Entities;

public sealed class Encounter
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public AdmissionType AdmissionType { get; set; }
    public EncounterStatus Status { get; set; }
    public ArrivalMode ArrivalMode { get; set; }
    public string ChiefComplaint { get; set; } = string.Empty;
    public Guid RegisteredBy { get; set; }
    public DateTimeOffset AdmittedAt { get; set; }
    public DateTimeOffset? DischargedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
