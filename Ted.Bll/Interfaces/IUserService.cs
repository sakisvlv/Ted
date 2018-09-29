using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model;
using Ted.Model.DTO;

namespace Ted.Bll.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserInfoDTO>> GetUserInfo(string userId);
        Task<Result<UserInfoDTO>> UpdateUserInfo(string userId, UserInfoDTO userInfo);
        Task<Result<bool>> UpdatePassword(string userId, ChangePasswordDTO userInfo);
        Task<Result<byte[]>> GetPhoto(string userId);
        Task<Result<string>> InsertPhoto(string userId, byte[] PhotoByteArray);
        Task<Result<SkillsDTO>> GetUserSkills(string userId);
        Task<Result<ExperienceDTO>> SaveExperience(string userId, ExperienceDTO experienceDTO);
        Task<Result<EducationDTO>> SaveEducation(string userId, EducationDTO educationDTO);
        Task<Result<PersonalSkillDTO>> SavePersonalSkill(string userId, PersonalSkillDTO personalSkillDTO);
        Task<Result<bool>> DeleteSkill(string userId, string type, string id);
        Task<Result<BudgiesDTO>> GetBudgies(string userId);
    }
}
