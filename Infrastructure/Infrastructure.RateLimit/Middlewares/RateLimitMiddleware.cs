using System.Net;
using Infrastructure.RateLimit.Models;
using Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.RateLimit.Middlewares
{
    public class RateLimitMiddleware
    {
        private const int Limit = 10;
        private const int Period = 1;
        private readonly RequestDelegate _next;
        private readonly ICacheService _cacheService;
        private readonly ILogger<RateLimitMiddleware> _logger;

        public RateLimitMiddleware(RequestDelegate next, ICacheService cacheService, ILogger<RateLimitMiddleware> logger)
        {
            _next = next;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var key = $"{context.Connection.RemoteIpAddress}_{context.Request.GetDisplayUrl()}";

            var result = await _cacheService.GetAsync<RateLimitModel>(key);

            if (result != null)
            {
                if (result.QuantityOfRequests == Limit && DateTime.UtcNow < result.TimeOfRequest.AddMinutes(Period))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    _logger.LogError($"Max 10 requests per 1 minute");
                    return;
                }
                else
                {
                    if (result!.QuantityOfRequests == Limit)
                    {
                        result.QuantityOfRequests = 0;
                    }

                    await _cacheService.AddOrUpdateAsync(key, new RateLimitModel { QuantityOfRequests = result.QuantityOfRequests + 1, TimeOfRequest = DateTime.UtcNow });
                }
            }
            else
            {
                await _cacheService.AddOrUpdateAsync(key, new RateLimitModel { QuantityOfRequests = 1, TimeOfRequest = DateTime.UtcNow });
            }

            await _next(context);
        }
    }
}