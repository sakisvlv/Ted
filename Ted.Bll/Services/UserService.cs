using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ted.Bll.Interfaces;
using Ted.Dal;
using Ted.Model;
using Ted.Model.Auth;
using Ted.Model.DTO;

namespace Ted.Bll.Services
{
    public class UserService : IUserService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public UserService(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Result<string>> InsertPhoto(string userId, byte[] PhotoByteArray)
        {
            var user = await _context.Users.Include(t => t.Photo).SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<string>.CreateFailed(
                    HttpStatusCode.NotFound, "User Does Not Exit");
            }

            Photo photo = new Photo();
            photo.File = PhotoByteArray;
            user.Photo = photo;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<string>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the photo");
            }
            return Result<string>.CreateSuccessful("Success");
        }
        public async Task<Result<byte[]>> GetPhoto(string userId)
        {
            var photo = await _context.Photos.SingleOrDefaultAsync(x => x.UserId == Guid.Parse(userId));
            if (photo == null)
            {
                return Result<byte[]>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't get the photo");
            }
            return Result<byte[]>.CreateSuccessful(photo.File);
        }

        public async Task<Result<UserInfoDTO>> GetUserInfo(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<UserInfoDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            return Result<UserInfoDTO>.CreateSuccessful(new UserInfoDTO(user));
        }

        public async Task<Result<UserInfoDTO>> UpdateUserInfo(string userId, UserInfoDTO userInfoDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<UserInfoDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            user.PhoneNumber = userInfoDTO.PhoneNumber;
            user.Email = userInfoDTO.Email;
            user.UserName = userInfoDTO.Email;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<UserInfoDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "Error Changes are not saved"); ;
            }

            return Result<UserInfoDTO>.CreateSuccessful(new UserInfoDTO(user));
        }

        public async Task<Result<bool>> UpdatePassword(string userId, ChangePasswordDTO passwords)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var changePassword = await _userManager.ChangePasswordAsync(user, passwords.OldPassword, passwords.NewPassword);
            if (!changePassword.Succeeded)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "Wrong old password or not proper new password");
            }
            
            return Result<bool>.CreateSuccessful(true);
        }
    }
}
