namespace TodoApi.Application.Abstractions
{
    public interface IIdempotencyService
    {
        Task<bool> ExistsAsync(string key, string route, CancellationToken ct = default);
        Task SetAsync(string key, string route, TimeSpan ttl, CancellationToken ct = default);
    }
}
