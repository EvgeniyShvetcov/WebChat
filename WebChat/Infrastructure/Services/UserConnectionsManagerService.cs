using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WebChat.Infrastructure.Services
{
    public class UserConnectionsManagerService
    {

        private readonly IList<WebSocket> _userConnections; 
        public IEnumerable<WebSocket> UserConnections => _userConnections.ToList();

        public UserConnectionsManagerService()
        {
            _userConnections = new List<WebSocket>();
        }

        public void Add(WebSocket webSocket)
        {
            _userConnections.Add(webSocket); 
        }

        public void Remove(WebSocket webSocket)
        {
            _userConnections.Remove(webSocket);
        }
    }
}
