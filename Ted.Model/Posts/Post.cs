using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.Posts
{
    public class Post : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public User Owner { get; set; }
        public IList<UserPost> UserPosts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public PostType Type { get; set; }
        public string FileName { get; set; }
        public DateTime PostedDate { get; set; }

        public Post(User owner, PostType type, string fileName, DateTime postedDate)
        {
            Owner = owner;
            Type = type;
            FileName = fileName;
            PostedDate = postedDate;
        }

        public Post()
        {
        }
    }
}
