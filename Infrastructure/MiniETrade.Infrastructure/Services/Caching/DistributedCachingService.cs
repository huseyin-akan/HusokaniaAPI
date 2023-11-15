using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MiniETrade.Application.Common.Abstractions.Caching;
using MiniETrade.Application.Common.Abstractions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.Caching
{
    public class DistributedCachingService : ICachingService
    {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        private readonly DistributedCachingOptions _cacheOptions;

        public DistributedCachingService(IConfiguration configuration, IDistributedCache cache, IOptions<DistributedCachingOptions> cachingOptions)
        {
            _configuration = configuration;
            _cache = cache;
            _cacheOptions = cachingOptions.Value;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var cachedData = await _cache.GetAsync(key, cancellationToken);

            if (cachedData != null)
            {
                var serializedData = Encoding.UTF8.GetString(cachedData);
                return JsonSerializer.Deserialize<T>(serializedData);
            }

            return default;
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        {
            var serializedData = JsonSerializer.Serialize(value);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                //1 saat boyunca cache'de kalıcak, 1 saat sonra mutlaka silinecek.
                AbsoluteExpiration = DateTime.Now.AddMinutes(_cacheOptions.AbsoluteExpirationMinutes),

                //Her 20 dakikada bir istek gelmezse 20 dakikanın sonunda bu data silinecek.
                SlidingExpiration = TimeSpan.FromMinutes(_cacheOptions.AbsoluteExpirationMinutes)
            };

            var bytes = Encoding.UTF8.GetBytes(serializedData);
            await _cache.SetAsync(key, bytes, cacheOptions, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(key, cancellationToken);
        }

        //We are adding caches to their group cache if they have, because if any of the cache group member changes, we want to remove all these cache members
        public async Task AddCacheKeyToGroup(ICachableRequest request, CancellationToken cancellationToken)
        {
            var cacheKeysInGroup = await GetAsync<HashSet<string>>(key: request.CacheGroupKey!, cancellationToken);

            if (cacheKeysInGroup != null && !cacheKeysInGroup.Contains(request.CacheKey))
                cacheKeysInGroup.Add(request.CacheKey);
            else
                cacheKeysInGroup = new HashSet<string>(new[] { request.CacheKey });

            await SetAsync(key: request.CacheGroupKey!, cacheKeysInGroup, cancellationToken);
        }
    }
}