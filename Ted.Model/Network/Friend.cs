using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.Network
{
    public class Friend
    {
        public Guid? FromUserId { set; get; }
        public virtual User FromUser { set; get; }
        public bool FromUserAccepted { get; set; }
        public Guid? ToUserId { set; get; }
        public bool ToUserAccepted { get; set; }
        public virtual User ToUser { set; get; }

        public Friend(User fromUser, User toUser)
        {
            FromUser = fromUser;
            ToUser = toUser;
        }

        public Friend()
        {
        }
    }
}
