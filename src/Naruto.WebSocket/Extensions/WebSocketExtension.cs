﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Naruto.WebSocket.Object;
using Naruto.WebSocket.Extensions;

namespace Naruto.WebSocket
{
    /// <summary>
    /// 张海波
    /// 2020-04-1
    /// 操作websocket的扩展
    /// </summary>
    internal static class WebSocketExtension
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="webSocketMessageType">消息类型 默认为文本</param>
        /// <param name="webSocket"></param>
        internal static async Task SendMessage(this System.Net.WebSockets.WebSocket webSocket, WebSocketMessageModel sendMessageModel, WebSocketMessageType webSocketMessageType = WebSocketMessageType.Text)
        {
            //验证连接是否正常
            if (webSocket.State != WebSocketState.Open)
                return;
            if (sendMessageModel.IsNull())
                return;
            //发送
            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(await sendMessageModel.ToJsonAsync())), webSocketMessageType, true, CancellationToken.None);
        }
    }
}
