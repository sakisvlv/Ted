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
    }
}
