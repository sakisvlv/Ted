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
        Task<Result<IEnumerable<UserListItemDTO>>> GetUsers(string userId);
        Task<Result<CountsDTO>> GetCounts(string userId);
    }
}
