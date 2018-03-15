using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebChat.Models;

namespace WebChat.Infrastructure.Services
{
    public class ChatWorkerService
    {
        private readonly UserConnectionsManagerService _userConnectionsManagerService;

        public ChatWorkerService(UserConnectionsManagerService userConnectionsManagerService)
        {
            _userConnectionsManagerService = userConnectionsManagerService;
        }

        public async Task SendMessage(MessageDTO message)
        {
            foreach(var connection in _userConnectionsManagerService.UserConnections)
            {
                var json = JsonConvert.SerializeObject(message);
                var arraySegment = new ArraySegment<byte>(Encoding.ASCII.GetBytes(json));
                await connection.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
