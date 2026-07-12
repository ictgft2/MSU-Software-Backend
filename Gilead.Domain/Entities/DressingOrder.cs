using Gilead.Domain.Enums;

namespace Gilead.Domain.Entities;

public sealed class DressingOrder
{
    public Guid Id { get; set; }
    public Guid ConsultationNoteId { get; set; }
    public Guid EncounterId { get; set; }
    public string Instructions { get; set; } = string.Empty;
    public DressingOrderStatus Status { get; set; }
    public Guid? PerformedBy { get; set; }
    public string? ProcedureNotes { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
