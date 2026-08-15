namespace Gilead.Domain.Entities;

public sealed class Dispensing
{
    public Guid Id { get; set; }
    public Guid PrescriptionId { get; set; }
    public Guid PharmacistId { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public int QuantityDispensed { get; set; }
    public string BatchNumber { get; set; } = string.Empty;
    public DateOnly ExpiryDate { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset DispensedAt { get; set; }
}
