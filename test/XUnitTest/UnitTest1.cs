using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Net.WebSockets;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace XUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            List<ClientWebSocket> s = new List<ClientWebSocket>();
            for (int i = 0; i < 5000; i++)
            {
                ClientWebSocket webSocket = new ClientWebSocket();
                s.Add(webSocket);
                await webSocket.ConnectAsync(new Uri("ws://localhost:5003/ws"), CancellationToken.None);
            }

        }

        [Fact]
        public async Task Test2()
        {
            ClientWebSocket webSocket = new ClientWebSocket();
            await webSocket.ConnectAsync(new Uri("ws://localhost:5003/ws"), CancellationToken.None);

            var msg = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { action = "current", connectionId = "12345678", msg = "hello word" }));
            for (int i = 0; i < 1000; i++)
            {
                await webSocket.SendAsync(msg, WebSocketMessageType.Text, true, CancellationToken.None);
            }
          
            await Task.Delay(100000);
        }
    }
}
