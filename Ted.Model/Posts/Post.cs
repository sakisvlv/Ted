using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.Posts
{
    public class Post : Entity
    {
        public string Title { get; set; }
        public User User { get; set; }
        public List<Guid> Subscribers { get; set; }
        public PostType Type { get; set; }
        public byte[] Content { get; set; }
        public DateTime PostedDate { get; set; }

        public Post(User owner, PostType type, byte[] content, DateTime postedDate)
        {
            User = owner;
            Type = type;
            Content = content;
            PostedDate = postedDate;
        }

        public Post()
        {
        }
    }
}
