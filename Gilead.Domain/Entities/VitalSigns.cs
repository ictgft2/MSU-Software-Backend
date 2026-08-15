namespace Gilead.Domain.Entities;

public sealed class VitalSigns
{
    public Guid Id { get; set; }
    public Guid EncounterId { get; set; }
    public Guid RecordedBy { get; set; }
    public int? BloodPressureSystolic { get; set; }
    public int? BloodPressureDiastolic { get; set; }
    public int? PulseRate { get; set; }
    public decimal? Temperature { get; set; }
    public int? Spo2 { get; set; }
    public int? RespiratoryRate { get; set; }
    public decimal? Weight { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset RecordedAt { get; set; }
}
