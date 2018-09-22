using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model;
using Ted.Model.DTO;

namespace Ted.Bll.Interfaces
{
    public interface INetworkService
    {
        Task<Result<IEnumerable<UserInfoSmallDTO>>> GetFriends(string userId);
        Task<Result<bool>> AddFriend(string userId, string friendId);
        Task<Result<IEnumerable<UserInfoSmallDTO>>> GetPendingFriends(string userId);
        Task<Result<IEnumerable<UserInfoSmallDTO>>> SearchFriends(string userId, string query);
        Task<Result<bool>> IsFriend(string userId, string requestUserId);
        Task<Result<bool>> AcceptFriend(string userId, string friendId);
        Task<Result<bool>> RejectFriend(string userId, string friendId);
    }
}
