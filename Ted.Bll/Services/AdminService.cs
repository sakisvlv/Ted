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
            var adminRoles = await _userManager.GetRolesAsync(user);
            if (!adminRoles.Contains("Admin"))
            {
                return Result<IEnumerable<UserListItemDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "Has not Admin Role");
            }

            var users = await _context.Users.ToListAsync();
            List<Tuple<User, string>> usersRoles = new List<Tuple<User, string>>();
            for (int i = 0; i < users.Count; i++)
            {
                var roles = await _userManager.GetRolesAsync(users[i]);
                var role = roles.ToList().LastOrDefault();
                usersRoles.Add(new Tuple<User, string>(users[i], role));
            }
            return Result<IEnumerable<UserListItemDTO>>.CreateSuccessful(UserListItemDTO.ToDeviceDTOList(usersRoles));

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

        public async Task<Result<byte[]>> GetPhoto(string adminId, string userId)
        {
            var admin = await _userManager.FindByIdAsync(adminId);
            if (admin == null)
            {
                return Result<byte[]>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            var roles = await _userManager.GetRolesAsync(admin);
            if (!roles.Contains("Admin"))
            {
                return Result<byte[]>.CreateFailed(
                   HttpStatusCode.NotFound, "Has not Admin Role");
            }

            var photo = await _context.Photos.SingleOrDefaultAsync(x => x.UserId == Guid.Parse(userId));
            if (photo == null)
            {
                return Result<byte[]>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't get the photo");
            }
            return Result<byte[]>.CreateSuccessful(photo.File);
        }

        public async Task<Result<UserListItemDTO>> GetUser(string adminId, string userId)
        {
            var admin = await _userManager.FindByIdAsync(adminId);
            if (admin == null)
            {
                return Result<UserListItemDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            var adminRoles = await _userManager.GetRolesAsync(admin);
            if (!adminRoles.Contains("Admin"))
            {
                return Result<UserListItemDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "Has not Admin Role");
            }

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<UserListItemDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.ToList().LastOrDefault();
            return Result<UserListItemDTO>.CreateSuccessful(new UserListItemDTO(user, role));
        }
    }
}
