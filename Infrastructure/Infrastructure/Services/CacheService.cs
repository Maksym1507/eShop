﻿using Infrastructure.Configurations;
using Infrastructure.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> _logger;
        private readonly IRedisCacheConnectionService _redisCacheConnectionService;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly RedisConfig _config;

        public CacheService(
            ILogger<CacheService> logger,
            IRedisCacheConnectionService redisCacheConnectionService,
            IJsonSerializer jsonSerializer,
            IOptions<RedisConfig> config)
        {
            _logger = logger;
            _redisCacheConnectionService = redisCacheConnectionService;
            _jsonSerializer = jsonSerializer;
            _config = config.Value;
        }

        public Task AddOrUpdateAsync<T>(string key, T value)
        => AddOrUpdateInternalAsync(key, value);

        public async Task<T> GetAsync<T>(string key)
        {
            var redis = GetRedisDatabase();

            var cacheKey = GetItemCacheKey(key);

            var serialized = await redis.StringGetAsync(cacheKey);

            return serialized.HasValue ?
                _jsonSerializer.Deserialize<T>(serialized.ToString())
                : default(T)!;
        }

        private string GetItemCacheKey(string userId) =>
            $"{userId}";

        private async Task AddOrUpdateInternalAsync<T>(
            string key,
            T value,
            StackExchange.Redis.IDatabase redis = null!,
            TimeSpan? expiry = null)
        {
            redis = redis ?? GetRedisDatabase();
            expiry = expiry ?? _config.CacheTimeout;

            var cacheKey = GetItemCacheKey(key);
            var serialized = _jsonSerializer.Serialize(value);

            if (await redis.StringSetAsync(cacheKey, serialized, expiry))
            {
                _logger.LogInformation($"Cached value for key {key} cached");
            }
            else
            {
                _logger.LogInformation($"Cached value for key {key} updated");
            }
        }

        private StackExchange.Redis.IDatabase GetRedisDatabase() => _redisCacheConnectionService.Connection.GetDatabase();
    }
}
