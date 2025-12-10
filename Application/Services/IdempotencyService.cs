using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using TodoApi.Application.Abstractions;

namespace TodoApi.Application.Services
{
    public class IdempotencyService : IIdempotencyService
    {
        private readonly IMemoryCache _cache;
        private static readonly ConcurrentDictionary<string, byte> _keys = new();
        public IdempotencyService(IMemoryCache cache) => _cache = cache;


        private static string Compose(string key, string route) => $"{route}::{key}".ToLowerInvariant();


        public Task<bool> ExistsAsync(string key, string route, CancellationToken ct = default)
        {
            var composed = Compose(key, route);
            return Task.FromResult(_cache.TryGetValue(composed, out _));
        }


        public Task SetAsync(string key, string route, TimeSpan ttl, CancellationToken ct = default)
        {
            var composed = Compose(key, route);
            _cache.Set(composed, true, ttl);
            _keys.TryAdd(composed, 1);
            return Task.CompletedTask;
        }
    }
}
