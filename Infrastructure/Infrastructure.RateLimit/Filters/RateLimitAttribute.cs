using System.Net;
using Infrastructure.RateLimit.Models;
using Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Infrastructure.RateLimit.Filters
{
    public class RateLimitAttribute : ActionFilterAttribute
    {
        private readonly int _limit;
        private readonly int _period;
        private readonly ICacheService _cacheService;
        private readonly ILogger<RateLimitAttribute> _logger;

        public RateLimitAttribute(int limit, int period, ICacheService cacheService, ILogger<RateLimitAttribute> logger)
        {
            _limit = limit;
            _period = period;
            _cacheService = cacheService;
            _logger = logger;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var key = $"{context.HttpContext.Connection.RemoteIpAddress}_{context.HttpContext.Request.GetDisplayUrl()}";

            var result = await _cacheService.GetAsync<RateLimitModel>(key);

            if (result != null)
            {
                if (result.QuantityOfRequests == _limit && DateTime.UtcNow < result.TimeOfRequest.AddMinutes(_period))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    _logger.LogError($"Max 10 requests per 1 minute");
                    return;
                }
                else
                {
                    if (result.QuantityOfRequests == _limit)
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

            await next();
        }
    }
}
