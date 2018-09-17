using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.Posts
{
    public class Comment : Entity
    {
        public User User { get; set; }
        public string Text { get; set; }
    }
}
