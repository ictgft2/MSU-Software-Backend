namespace Gilead.Domain.Entities;

public sealed class LabResult
{
    public Guid Id { get; set; }
    public Guid LabRequestId { get; set; }
    public Guid ScientistId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public string Findings { get; set; } = string.Empty;
    public string Conclusion { get; set; } = string.Empty;
    public string Values { get; set; } = "[]";
    public DateTimeOffset CompletedAt { get; set; }
}
