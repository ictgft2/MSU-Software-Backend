namespace Gilead.Domain.Entities;

public sealed class ServiceTimeWindow
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly ColdCaseOpenTime { get; set; }
    public TimeOnly ColdCaseCloseTime { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
