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
        public IList<UserPost> UserPosts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public PostType Type { get; set; }
        public byte[] Content { get; set; }
        public DateTime PostedDate { get; set; }

        public Post(User owner, PostType type, byte[] content, DateTime postedDate)
        {
            Owner = owner;
            Type = type;
            Content = content;
            PostedDate = postedDate;
        }

        public Post()
        {
        }
    }
}
