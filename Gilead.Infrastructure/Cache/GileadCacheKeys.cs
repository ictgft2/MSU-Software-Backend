namespace Gilead.Infrastructure.Cache;

public static class GileadCacheKeys
{
    public static string Queue(DateOnly date) => $"gilead:queue:{date:yyyy-MM-dd}";
}
