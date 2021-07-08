using ChatAPI.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Hubs
{
    public class ChatHub : Hub
    {
        private static List<User> ConnectedUsers = new List<User>();

        public Task SendMessage(string senderName, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", senderName, message);
        }

        public Task SendMessageToUser(string userId, string senderName, string message)
        {
            Console.WriteLine(userId + " " + senderName + " " + message);
            return Clients.Client(userId.Trim()).SendAsync("PrivateMessage", senderName, message);
        }

        public static List<User> GetUsersList()
        {
            return ConnectedUsers;
        }

        private Task UpdateUsersStatusList(List<User> connectedUsers)
        {
            return Clients.All.SendAsync("ConnectedUsers", connectedUsers);
        }

        public override Task OnConnectedAsync()
        {
            ConnectedUsers.Add(new User() { connectionId = Context.ConnectionId, userName = Context.User.Identity.Name });
            UpdateUsersStatusList(ConnectedUsers);
            Console.WriteLine($"User Connected: {Context.ConnectionId}, {Context.User.Identity.Name}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var itemPosition = ConnectedUsers.FindIndex(x => x.connectionId == Context.ConnectionId);
            ConnectedUsers.RemoveAt(itemPosition);
            UpdateUsersStatusList(ConnectedUsers);
            Console.WriteLine($"User Disconnected: {Context.ConnectionId}, {Context.User.Identity.Name}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
