﻿using Microsoft.Extensions.DependencyInjection;
using Naruto.Redis.RedisConfig;
using Naruto.WebSocket.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket.Redis
{
    /// <summary>
    /// 
    /// </summary>
    public static class NarutoWebSocketRedisExtension
    {
        /// <summary>
        ///以redis为底板实现 websocket的分布式环境
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddNarutoWebSocketRedis(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddService();
            return serviceDescriptors;
        }
        /// <summary>
        ///以redis为底板实现 websocket的分布式环境
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddNarutoWebSocketRedis(this IServiceCollection serviceDescriptors, Action<RedisOptions> option)
        {
            serviceDescriptors.AddRedisRepository(option);
            serviceDescriptors.AddService();
            return serviceDescriptors;
        }

        private static IServiceCollection AddService(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSingleton<IEventBus, RedisEventBus>();
            serviceDescriptors.AddSingleton<ISubscribeMessageStorage, RedisSubscribeMessageStorage>();
            serviceDescriptors.BuildServiceProvider().GetRequiredService<IEventBus>().SubscribeMessageAsync().Wait();
            return serviceDescriptors;

        }
    }
}
