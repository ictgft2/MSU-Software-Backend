using Gilead.Domain.Enums;
using System.Text.Json.Serialization;

namespace Gilead.Domain.Entities;

public sealed class Staff
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public StaffRole Role { get; set; }
    public bool IsActive { get; set; }
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}
