using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Ted.Bll.Interfaces;
using Ted.Dal;
using Ted.Model;
using Ted.Model.Auth;
using Ted.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace Ted.Bll.Services
{
    public class NetworkService : INetworkService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public NetworkService(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result<IEnumerable<UserInfoSmallDTO>>> GetFriends(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<IEnumerable<UserInfoSmallDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            var friendlist = await _context.Friends
                .Where(x => (x.FromUser == user || x.ToUser == user) && (x.FromUserAccepted && x.ToUserAccepted))
                .Include(x => x.FromUser)
                .Include(x => x.ToUser)
                .ToListAsync();

            if (friendlist == null)
            {
                return Result<IEnumerable<UserInfoSmallDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var friends = new List<User>();
            foreach (var friend in friendlist)
            {
                if (friend.ToUser == user)
                {
                    friends.Add(friend.FromUser);
                    continue;
                }
                friends.Add(friend.ToUser);
            }
            return Result<IEnumerable<UserInfoSmallDTO>>.CreateSuccessful(UserInfoSmallDTO.ToUserInfoSmallDTOList(friends));
        }

        public async Task<Result<IEnumerable<UserInfoSmallDTO>>> AddFriend(string userId, string friendId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<IEnumerable<UserInfoSmallDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            var friendlist = await _context.Friends
                .Where(x => (x.FromUser == user || x.ToUser == user) && (x.FromUserAccepted && x.ToUserAccepted))
                .Include(x => x.FromUser)
                .Include(x => x.ToUser)
                .ToListAsync();

            if (friendlist == null)
            {
                return Result<IEnumerable<UserInfoSmallDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var friends = new List<User>();
            foreach (var friend in friendlist)
            {
                if (friend.ToUser == user)
                {
                    friends.Add(friend.FromUser);
                    continue;
                }
                friends.Add(friend.ToUser);
            }
            return Result<IEnumerable<UserInfoSmallDTO>>.CreateSuccessful(UserInfoSmallDTO.ToUserInfoSmallDTOList(friends));
        }
    }
}
