using System;
using System.Collections.Generic;
using System.Text;

namespace Ted.Model.DTO
{
    public class BudgiesDTO
    {
        public int FriendRequests { get; set; }
        public int Notifications { get; set; }
        public int Messages { get; set; }

        public BudgiesDTO(int friendRequests, int notifications, int messages)
        {
            FriendRequests = friendRequests;
            Notifications = notifications;
            Messages = messages;
        }
    }

}
