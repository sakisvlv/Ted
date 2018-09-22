using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model;
using Ted.Model.DTO;

namespace Ted.Bll.Interfaces
{
    public interface IViewService
    {
        Task<Result<UserInfoSmallDTO>> GetUserInfo(string userId, string requestUserId);
        Task<Result<int>> GetConnectionsCount(string userId, string requestUserId);
        Task<Result<SkillsDTO>> GetUserSkills(string userId, string requestUserId);
        Task<Result<bool>> IsFriend(string userId, string requestUserId);
    }
}
