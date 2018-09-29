using System;
using System.Collections.Generic;
using System.Text;

namespace Ted.Model.Ads
{
    public class AdKnn : Entity
    {
        public Ad Ad { get; set; }
        public GlobalString GlobalString { get; set; }
        public int Count { get; set; }
        public int Weight { get; set; }
    }
}
