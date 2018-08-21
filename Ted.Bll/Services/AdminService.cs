using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Ted.Bll.Interfaces;
using Ted.Dal;
using Ted.Model;
using Ted.Model.Auth;
using Ted.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace Ted.Bll.Services
{
    public class AdminService : IAdminService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public AdminService(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result<IEnumerable<UserListItemDTO>>> GetUsers(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<IEnumerable<UserListItemDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Admin"))
            {
                return Result<IEnumerable<UserListItemDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "Has not Admin Role");
            }

            var users = await _context.Users.ToListAsync();
            return Result<IEnumerable<UserListItemDTO>>.CreateSuccessful(UserListItemDTO.ToDeviceDTOList(users));

        }

        public async Task<Result<CountsDTO>> GetCounts(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<CountsDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Admin"))
            {
                return Result<CountsDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "Has not Admin Role");
            }

            var usersCount = await _context.Users.CountAsync();
            var counts = new CountsDTO();
            counts.Users = usersCount;
            return Result<CountsDTO>.CreateSuccessful(counts);

        }
    }
}
