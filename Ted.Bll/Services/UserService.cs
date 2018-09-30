using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ted.Bll.Interfaces;
using Ted.Dal;
using Ted.Model;
using Ted.Model.Auth;
using Ted.Model.DTO;
using Ted.Model.PersonalSkills;
using Ted.Knn;

namespace Ted.Bll.Services
{
    public class UserService : IUserService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IKnnService _knnService;

        public UserService(Context context, UserManager<User> userManager, IKnnService knnService)
        {
            _context = context;
            _userManager = userManager;
            _knnService = knnService;
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

        public async Task<Result<SkillsDTO>> GetUserSkills(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<SkillsDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var expiriences = await _context.Experiences.Where(x => x.User == user).ToListAsync();
            var educations = await _context.Educations.Where(x => x.User == user).ToListAsync();
            var personalSkills = await _context.PersonalSkills.Where(x => x.User == user).ToListAsync();
            var a = Result<SkillsDTO>.CreateSuccessful(
                new SkillsDTO(
                    ExperienceDTO.ToExperienceDTOList(expiriences),
                    EducationDTO.ToEducationDTOList(educations),
                    PersonalSkillDTO.ToPersonalSkillDTOList(personalSkills)
                    ));
            return a;
        }

        public async Task<Result<ExperienceDTO>> SaveExperience(string userId, ExperienceDTO experienceDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<ExperienceDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var expirience = await _context.Experiences.Where(x => x.User == user && x.Id == experienceDTO.Id).FirstOrDefaultAsync();
            
            if (expirience != null)
            {
                expirience.Update(experienceDTO);
                await _knnService.RemoveSkill(expirience, user);
            }
            else
            {
                expirience = new Experience();
                expirience.Update(experienceDTO);
                expirience.User = user;
                await _context.Experiences.AddAsync(expirience);
            }

            if (expirience.StillThere == true)
            {
                user.CurrentState = expirience.Description + " at " + expirience.Company;
            }

            await _knnService.ManageSkill(expirience, user);


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<ExperienceDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "Error Changes are not saved"); ;
            }

            return Result<ExperienceDTO>.CreateSuccessful(new ExperienceDTO(expirience));
        }

        public async Task<Result<EducationDTO>> SaveEducation(string userId, EducationDTO educationDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<EducationDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var education = await _context.Educations.Where(x => x.User == user && x.Id == educationDTO.Id).FirstOrDefaultAsync();
            if (education != null)
            {
                education.Update(educationDTO);
                await _knnService.RemoveSkill(education, user);
            }
            else
            {
                education = new Education();
                education.Update(educationDTO);
                education.User = user;
                await _context.Educations.AddAsync(education);
            }

            await _knnService.ManageSkill(education, user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<EducationDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "Error Changes are not saved"); ;
            }

            return Result<EducationDTO>.CreateSuccessful(new EducationDTO(education));
        }

        public async Task<Result<PersonalSkillDTO>> SavePersonalSkill(string userId, PersonalSkillDTO personalSkillDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<PersonalSkillDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var personalSkill = await _context.PersonalSkills.Where(x => x.User == user && x.Id == personalSkillDTO.Id).FirstOrDefaultAsync();
            if (personalSkill != null)
            {
                personalSkill.Update(personalSkillDTO);
                await _knnService.RemoveSkill(personalSkill, user);
            }
            else
            {
                personalSkill = new PersonalSkill();
                personalSkill.Update(personalSkillDTO);
                personalSkill.User = user;
                await _context.PersonalSkills.AddAsync(personalSkill);
            }

            await _knnService.ManageSkill(personalSkill, user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<PersonalSkillDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "Error Changes are not saved"); ;
            }

            return Result<PersonalSkillDTO>.CreateSuccessful(new PersonalSkillDTO(personalSkill));
        }

        public async Task<Result<bool>> DeleteSkill(string userId, string type, string id)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            switch (type)
            {
                case "experience":
                    var expToDelete = await _context.Experiences.Where(x => x.Id.ToString() == id).FirstOrDefaultAsync();
                    _context.Experiences.Remove(expToDelete);
                    break;
                case "education":
                    var edToDelete = await _context.Educations.Where(x => x.Id.ToString() == id).FirstOrDefaultAsync();
                    _context.Educations.Remove(edToDelete);
                    break;
                case "personalSkill":
                    var pSkToDelete = await _context.PersonalSkills.Where(x => x.Id.ToString() == id).FirstOrDefaultAsync();
                    _context.PersonalSkills.Remove(pSkToDelete);
                    break;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "Error Changes are not saved"); ;
            }

            return Result<bool>.CreateSuccessful(true);
        }

        public async Task<Result<BudgiesDTO>> GetBudgies(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<BudgiesDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            int friendRequests, notifications, messages;

            var friendlist = await _context.Friends
                .Where(x => (x.FromUser == user && !x.FromUserAccepted) || (x.ToUser == user && !x.ToUserAccepted))
                .Include(x => x.FromUser)
                .Include(x => x.ToUser)
                .ToListAsync();

            if (friendlist == null)
            {
                return Result<BudgiesDTO>.CreateFailed(
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
            friendRequests = friends.Count();

            notifications = await _context.Notifications
                .Where(x => x.ToUser.Id == Guid.Parse(userId) && x.IsAcknowledged == false).CountAsync();

            messages = await _context.Conversations
                .Where(x => (x.FromUser == user && x.FromUserHasNewMessages == true) || (x.ToUser == user && x.ToUserHasNewMessages == true))
                .CountAsync();

            
            return Result<BudgiesDTO>.CreateSuccessful(new BudgiesDTO(friendRequests, notifications, messages));
        }
    }
}
