﻿using Naruto.WebSocket.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.WebSocket.Interface.Client
{
    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 给所有客户端发送消息接口
    /// 用于集群环境中订阅发送
    /// </summary>
    public interface IClusterAllClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="execAction">调用的方法</param>
        /// <param name="msg">消息内容</param>
        /// <returns></returns>
        Task SendMessageAsync(string execAction, object msg);
    }

    /// <summary>
    /// 张海波
    /// 2020-04-04
    /// 给所有客户端发送消息接口
    ///  用于集群环境中订阅发送
    /// </summary>
    public interface IClusterAllClient<TService> : IClusterAllClient where TService : NarutoWebSocketService
    {

    }
}
