using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.Posts
{
    public class Post : Entity
    {
        public string Title { get; set; }
        public User Owner { get; set; }
        public List<User> Subscribers { get; set; }
        public PostType Type { get; set; }
        public byte[] Content { get; set; }
    }
}
