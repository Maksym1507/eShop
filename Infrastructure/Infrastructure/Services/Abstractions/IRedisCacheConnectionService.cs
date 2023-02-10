using StackExchange.Redis;

namespace Infrastructure.Services.Abstractions
{
    public interface IRedisCacheConnectionService
    {
        IConnectionMultiplexer Connection { get; }
    }
}
