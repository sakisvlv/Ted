using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.Conversations
{
    public class Message : Entity
    {
        public Conversation Conversation { get; set; }
        public User Sender { get; set; }
        public string Text { get; set; }
        public DateTime DateSended { get; set; }
    }
}
