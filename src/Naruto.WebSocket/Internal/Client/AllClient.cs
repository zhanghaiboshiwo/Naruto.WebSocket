﻿using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Interface.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Naruto.WebSocket.Object;
using Naruto.WebSocket.Internal.Cache;
using Naruto.WebSocket.Object.Enums;
using Microsoft.Extensions.Logging;
using Naruto.WebSocket.Extensions;

namespace Naruto.WebSocket.Internal.Client
{
    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 单机版给所有人发消息
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public sealed class AllClient<TService> : IAllClient<TService>, IClusterAllClient<TService> where TService : NarutoWebSocketService
    {
        /// <summary>
        /// webscoket 客户端存储
        /// </summary>
        private readonly IWebSocketClientStorage socketClientStorage;

        private readonly ILogger<AllClient<TService>> logger;

        /// <summary>
        /// 定义一个当前服务对应的租户请求path字段
        /// </summary>
        private readonly string RequestPath;
        /// <summary>
        /// 事件总线代理对象
        /// </summary>
        private readonly IEventBusProxy eventBusProxy;

        public AllClient(IWebSocketClientStorage<TService> _socketClientStorage, IEventBusProxy _eventBusProxy, ILogger<AllClient<TService>> _logger)
        {
            socketClientStorage = _socketClientStorage;
            RequestPath = TenantPathCache.GetByType(typeof(TService));
            eventBusProxy = _eventBusProxy;
            logger = _logger;
        }

        public async Task SendAsync(string execAction, object msg)
        {
            await SendMessageAsync(execAction, msg);
            //发布事件
            await eventBusProxy.PublishAsync(new SubscribeMessage
            {
                TenantIdentity = RequestPath,
                SendTypeEnum = MessageSendTypeEnum.All,
                ParamterEntity = new ParamterEntity
                {
                    Message = new WebSocketMessageModel
                    {
                        action = execAction,
                        message = msg
                    }
                }
            });
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="execAction">调用的方法</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(string execAction, object msg)
        {
            logger.LogTrace("发送消息给所有的在线用户,execAction={execAction", execAction);
            //获取所有在线的用户
            var webSockets = await socketClientStorage.GetAllAsync();
            Parallel.ForEach(webSockets, async item =>
            {
                await item.SendMessage(new WebSocketMessageModel
                {
                    action = execAction,
                    message = msg
                });
            });
        }
    }
}
