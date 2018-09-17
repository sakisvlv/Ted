using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.Posts
{
    public class UserPost
    {
        public Guid UserId { set; get; }
        public User User { set; get; }
        public Guid PostId { set; get; }
        public Post Post { set; get; }

        public UserPost(User user, Post post)
        {
            User = user;
            Post = post;
        }

        public UserPost()
        {
        }
    }
}
