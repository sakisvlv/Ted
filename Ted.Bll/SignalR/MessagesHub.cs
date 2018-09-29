using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ted.Model.Auth;

namespace Ted.Bll.SignalR
{
    [Authorize]
    public class MessagesHub : Hub
    {
        public Task SendPrivateMessage(string user, string message, string conversationId)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message, conversationId);
        }

        //public override async Task OnConnectedAsync()
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
        //    await base.OnConnectedAsync();
        //}
    }
}
