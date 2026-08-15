using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using StackExchange.Redis;

namespace Gilead.Infrastructure.Cache;

public sealed class QueueCacheService(IConnectionMultiplexer redis) : IQueueCacheService
{
    public async Task JoinAsync(Guid encounterId, DateOnly date, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var score = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        await redis.GetDatabase().SortedSetAddAsync(GileadCacheKeys.Queue(date), encounterId.ToString(), score);
    }

    public async Task DequeueAsync(Guid encounterId, DateOnly date, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await redis.GetDatabase().SortedSetRemoveAsync(GileadCacheKeys.Queue(date), encounterId.ToString());
    }

    public async Task<long?> GetPositionAsync(Guid encounterId, DateOnly date, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var rank = await redis.GetDatabase().SortedSetRankAsync(GileadCacheKeys.Queue(date), encounterId.ToString(), Order.Ascending);
        return rank is null ? null : rank.Value + 1;
    }

    public async Task<IReadOnlyList<QueueEntry>> GetFullListAsync(DateOnly date, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var rows = await redis.GetDatabase().SortedSetRangeByScoreWithScoresAsync(GileadCacheKeys.Queue(date), order: Order.Ascending);
        return rows.Select((row, index) => new QueueEntry(Guid.Parse(row.Element!), row.Score, index + 1)).ToArray();
    }
}
