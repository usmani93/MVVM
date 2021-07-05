using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Hubs
{
    public class ChatHub : Hub
    {
        private static List<string> ConnectedUsers = new List<string>();

        public Task SendMessage(string senderName, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", senderName, message);
        }

        public Task SendMessageToUser(string userId, string senderName, string message)
        {
            Console.WriteLine(userId + " " + senderName + " " + message);
            return Clients.Client(userId.Trim()).SendAsync("PrivateMessage", senderName, message);
        }

        public static List<string> GetUsersList()
        {
            return ConnectedUsers;
        }

        private Task UpdateUsersStatusList(List<string> connectedUsers)
        {
            return Clients.All.SendAsync("ConnectedUsers", connectedUsers);
        }

        public override Task OnConnectedAsync()
        {
            ConnectedUsers.Add(Context.ConnectionId);
            UpdateUsersStatusList(ConnectedUsers);
            Console.WriteLine($"User Connected: {Context.ConnectionId}, {Context.User}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUsers.Remove(Context.ConnectionId);
            UpdateUsersStatusList(ConnectedUsers);
            Console.WriteLine($"User Connected: {Context.ConnectionId}, {Context.User}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
