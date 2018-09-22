using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ted.Dal;
using Ted.Model.Auth;

namespace Ted.Bll
{
    public static class Helpers
    {
        public static async Task<List<User>> GetAllFriends(Context context, User requestUser)
        {

            var friendlist = await context.Friends
               .Where(x => (x.FromUser == requestUser || x.ToUser == requestUser) && (x.FromUserAccepted && x.ToUserAccepted))
               .Include(x => x.FromUser)
               .Include(x => x.ToUser)
               .ToListAsync();

            var friends = new List<User>();
            foreach (var friend in friendlist)
            {
                if (friend.ToUser == requestUser)
                {
                    friends.Add(friend.FromUser);
                    continue;
                }
                friends.Add(friend.ToUser);
            }
            return friends;
        }
    }
}
