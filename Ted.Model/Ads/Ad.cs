using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.Ads
{
    public class Ad : Entity
    {
        public User Owner { get; set; }
        public ICollection<User> Applicants { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
