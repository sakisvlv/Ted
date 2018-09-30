using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Ads;

namespace Ted.Model.Posts
{
    public class Pknn : Entity
    {
        public Post Post { get; set; }
        public GlobalString GlobalString { get; set; }
        public int Count { get; set; }
    }
}
