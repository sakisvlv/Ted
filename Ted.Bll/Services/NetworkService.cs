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
using Ted.Model.Network;
using Ted.Model.Notifications;
using Microsoft.AspNetCore.SignalR;
using Ted.Bll.SignalR;

namespace Ted.Bll.Services
{
    public class NetworkService : INetworkService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<BudgiesHub> _hubContext;

        public NetworkService(Context context, UserManager<User> userManager, IHubContext<BudgiesHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
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

        public async Task<Result<IEnumerable<UserInfoSmallDTO>>> GetPendingFriends(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<IEnumerable<UserInfoSmallDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            var friendlist = await _context.Friends
                .Where(x => (x.FromUser == user && !x.FromUserAccepted) || (x.ToUser == user && !x.ToUserAccepted))
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

        public async Task<Result<IEnumerable<UserInfoSmallDTO>>> SearchFriends(string userId, string query)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<IEnumerable<UserInfoSmallDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            query = query.ToLower();

            var array = query.Split(' ');

            var result = await _context.Users.Where(x =>
            array.Contains(x.FirstName.ToLower()) ||
            array.Contains(x.LastName.ToLower()) ||
            array.Contains(x.Email.ToLower())).ToListAsync();

            return Result<IEnumerable<UserInfoSmallDTO>>.CreateSuccessful(UserInfoSmallDTO.ToUserInfoSmallDTOList(result));
        }

        public async Task<Result<bool>> AddFriend(string userId, string friendId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var friend = await _userManager.FindByIdAsync(friendId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var friends = new Friend();
            friends.FromUser = user;
            friends.FromUserAccepted = true;
            friends.ToUser = friend;
            friends.ToUserAccepted = false;

            _context.Friends.Add(friends);

            var notification = new Notification
            {
                IsAcknowledged = false,
                PostId = null,
                Sender = user.FirstName + " " + user.LastName,
                SenderId = user.Id,
                ToUser = friend,
                Type = NotificationType.FriendRequest,
                DateAdded = DateTime.Now
            };
            _context.Notifications.Add(notification);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't add friend");
            }

            await _hubContext.Clients.User(friend.Id.ToString()).SendAsync("CheckBudgies", "FriendRequest");

            return Result<bool>.CreateSuccessful(true);
        }

        public async Task<Result<bool>> AcceptFriend(string userId, string friendId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var friend = await _userManager.FindByIdAsync(friendId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var request = await _context.Friends
                .Where(x => x.ToUser == user &&
                x.ToUserAccepted == false
                && x.FromUser == friend
                && x.FromUserAccepted == true)
                .FirstOrDefaultAsync();
            request.ToUserAccepted = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't add friend");
            }

            return Result<bool>.CreateSuccessful(true);
        }

        public async Task<Result<bool>> RejectFriend(string userId, string friendId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var friend = await _userManager.FindByIdAsync(friendId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var request = await _context.Friends
                .Where(x => x.ToUser == user &&
                x.ToUserAccepted == false
                && x.FromUser == friend
                && x.FromUserAccepted == true)
                .FirstOrDefaultAsync();

            _context.Friends.Remove(request);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't add friend");
            }

            return Result<bool>.CreateSuccessful(true);
        }

        public async Task<Result<bool>> IsFriend(string userId, string requestUserId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var requestUser = await _userManager.FindByIdAsync(requestUserId);
            if (requestUser == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var result = await _context.Friends.Where(x => (x.FromUser == user && x.ToUser == requestUser) || (x.FromUser == requestUser && x.ToUser == user)).ToListAsync();


            return Result<bool>.CreateSuccessful(result.Count == 1);
        }
    }
}
