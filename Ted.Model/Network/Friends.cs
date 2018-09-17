using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.Network
{
    public class Friends
    {
        public Guid UserId1 { set; get; }
        public User User1 { set; get; }
        public Guid UserId2 { set; get; }
        public User User2 { set; get; }

        public Friends(User user1, User user2)
        {
            User1 = user1;
            User1 = user2;
        }

        public Friends()
        {
        }
    }
}
