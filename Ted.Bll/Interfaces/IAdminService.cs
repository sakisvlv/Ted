using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model;
using Ted.Model.DTO;

namespace Ted.Bll.Interfaces
{
    public interface IAdminService
    {
        Task<Result<UserListItemDTO>> GetUser(string adminId, string userId);
        Task<Result<IEnumerable<UserListItemDTO>>> GetUsers(string userId);
        Task<Result<CountsDTO>> GetCounts(string userId);
        Task<Result<string>> GetXml(string adminId, List<string> userIds);
        Task<Result<byte[]>> GetPhoto(string adminId, string userId);
    }
}
