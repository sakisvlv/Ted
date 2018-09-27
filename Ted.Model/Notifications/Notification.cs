using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.Notifications
{
    public class Notification : Entity
    {
        public User ToUser { set; get; }
        public string Sender { set; get; }
        public Guid SenderId { set; get; }
        public NotificationType Type { get; set; }
        public Guid? PostId { set; get; }
        public bool IsAcknowledged { set; get; }
        public DateTime DateAdded { set; get; }
    }
}
