using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebChat.Infrastructure.Services;
using WebChat.Models;

namespace WebChat.Infrastructure.MiddleWares
{
    public class WebSocketHandleMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly UserConnectionsManagerService _connectionsManagerService;
        private readonly ChatWorkerService _chatWorkerService;

        public WebSocketHandleMiddleWare(RequestDelegate next, UserConnectionsManagerService connectionsManagerService, ChatWorkerService chatWorkerService)
        {
            _next = next;
            _connectionsManagerService = connectionsManagerService;
            _chatWorkerService = chatWorkerService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                _connectionsManagerService.Add(socket);
                var buffer = new byte[4096];
                while (!socket.CloseStatus.HasValue)
                {
                    var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                    var mesasge = JsonConvert.DeserializeObject<MessageDTO>(Encoding.UTF8.GetString(buffer));
                    await _chatWorkerService.SendMessage(mesasge);
                }

                _connectionsManagerService.Remove(socket);
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}

