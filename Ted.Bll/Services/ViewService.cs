using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ViewService : IViewService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly string _filePath;

        public ViewService(IConfiguration configuration, Context context, UserManager<User> userManager)
        {
            _filePath = configuration.GetSection("filePath").Value;
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result<UserInfoSmallDTO>> GetUserInfo(string userId, string requestUserId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<UserInfoSmallDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var requestUser = await _userManager.FindByIdAsync(requestUserId);
            if (requestUser == null)
            {
                return Result<UserInfoSmallDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            return Result<UserInfoSmallDTO>.CreateSuccessful(new UserInfoSmallDTO(requestUser));
        }

        public async Task<Result<int>> GetConnectionsCount(string userId, string requestUserId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<int>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var requestUser = await _userManager.FindByIdAsync(requestUserId);
            if (requestUser == null)
            {
                return Result<int>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var result = await Helpers.GetAllFriends(_context, requestUser);

            return Result<int>.CreateSuccessful(result.Count);
        }

        public async Task<Result<SkillsDTO>> GetUserSkills(string userId, string requestUserId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<SkillsDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var requestUser = await _userManager.FindByIdAsync(requestUserId);
            if (requestUser == null)
            {
                return Result<SkillsDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var expiriences = await _context.Experiences.Where(x => x.User == requestUser).ToListAsync();
            var educations = await _context.Educations.Where(x => x.User == requestUser).ToListAsync();
            var personalSkills = await _context.PersonalSkills.Where(x => x.User == requestUser).ToListAsync();
            var a = Result<SkillsDTO>.CreateSuccessful(
                new SkillsDTO(
                    ExperienceDTO.ToExperienceDTOList(expiriences),
                    EducationDTO.ToEducationDTOList(educations),
                    PersonalSkillDTO.ToPersonalSkillDTOList(personalSkills)
                    ));
            return a;
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
