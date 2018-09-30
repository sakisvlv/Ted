using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.Ads
{
    public class UserAd
    {
        public Guid UserId { set; get; }
        public User User { set; get; }
        public Guid AdId { set; get; }
        public Ad Ad { set; get; }

        public UserAd(User user, Ad ad)
        {
            User = user;
            Ad = ad;
        }

        public UserAd()
        {
        }
    }
}
