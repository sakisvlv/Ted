using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class HomeService : IHomeService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public HomeService(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result<ExperienceDTO>> GetLastExperience(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<ExperienceDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            var experience = await _context.Experiences.Where(x => x.StillThere == true && x.User == user).FirstOrDefaultAsync();
            if (experience == null)
            {
                return Result<ExperienceDTO>.CreateSuccessful(new ExperienceDTO());
            }
            return Result<ExperienceDTO>.CreateSuccessful(new ExperienceDTO(experience));
        }

        public async Task<Result<string>> InsertImage(string userId, byte[] ImageByteArray)
        {
            var user = await _context.Users.Include(t => t.Photo).SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<string>.CreateFailed(
                    HttpStatusCode.NotFound, "User Does Not Exit");
            }

            Photo photo = new Photo();
            photo.File = ImageByteArray;
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

    }
}
