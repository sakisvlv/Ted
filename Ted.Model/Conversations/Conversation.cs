using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.Conversations
{
    public class Conversation : Entity
    {
        public Guid? FromUserId { set; get; }
        public virtual User FromUser { set; get; }
        public Guid? ToUserId { set; get; }
        public virtual User ToUser { set; get; }
        public List<Message> Messages { get; set; }
        public bool FromUserHasNewMessages { get; set; }
        public bool ToUserHasNewMessages { get; set; }
        public DateTime LastMessageDate { get; set; }

        public Conversation(User fromUser, User toUser)
        {
            FromUser = fromUser;
            ToUser = toUser;
        }

        public Conversation()
        {
        }
    }
}
