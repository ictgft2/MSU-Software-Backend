namespace Gilead.Domain.Entities;

public sealed class LabResultValue
{
    public string Parameter { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string ReferenceRange { get; set; } = string.Empty;
}
