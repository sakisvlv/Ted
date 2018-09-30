using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ted.Bll.SignalR
{
    [Authorize]
    public class BudgiesHub : Hub
    {
        public Task SendPrivateMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("CheckBudgies", message);
        }
    }
}
