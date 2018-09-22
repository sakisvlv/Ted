using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.Notifications
{
    public class Notification : Entity
    {
        public Guid? FromUserId { set; get; }
        public virtual User FromUser { set; get; }
        public Guid? ToUserId { set; get; }
        public virtual User ToUser { set; get; }
        public NotificationType Type { get; set; }
        public Guid? PostId { set; get; }
    }
}
