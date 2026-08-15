namespace Gilead.Domain.Entities;

public sealed class Patient
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Sex { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string NextOfKinName { get; set; } = string.Empty;
    public string NextOfKinPhone { get; set; } = string.Empty;
    public string NextOfKinRelationship { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}
