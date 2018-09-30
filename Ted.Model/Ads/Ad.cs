using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Ted.Model.Auth;

namespace Ted.Model.Ads
{
    public class Ad : Entity
    {
        public User Owner { get; set; }
        [XmlIgnore]
        public IList<UserAd> UserAds { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
        [XmlIgnore]
        public ICollection<AdKnn> AdKnns { get; set; }
    }
}
