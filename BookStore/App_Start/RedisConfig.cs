using StackExchange.Redis;
using System;

namespace BookStore.App_Start
{
    public class RedisConfig
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect("redis-epm-cdp.redis.cache.windows.net:6380,password=ikhuzdMZQnfGp3o0Urblkv9nXmygmxE4RhNBhdshZ9M=,ssl=True,abortConnect=False");
        });

        public static ConnectionMultiplexer RedisConnection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}