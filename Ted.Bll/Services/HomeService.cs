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
using Ted.Model.Posts;

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

        public async Task<Result<string>> InsertImage(string userId, byte[] imageByteArray)
        {
            var user = await _context.Users.Include(t => t.Photo).SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<string>.CreateFailed(
                    HttpStatusCode.NotFound, "User Does Not Exit");
            }

            Post post = new Post(user, PostType.Image, imageByteArray, DateTime.Now);
            await _context.Posts.AddAsync(post);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<string>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the photo");
            }
            return Result<string>.CreateSuccessful(post.Id.ToString());
        }

        public async Task<Result<string>> InsertVideo(string userId, byte[] videoByteArray)
        {
            var user = await _context.Users.Include(t => t.Photo).SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<string>.CreateFailed(
                    HttpStatusCode.NotFound, "User Does Not Exit");
            }

            Post post = new Post(user, PostType.Video, videoByteArray, DateTime.Now);
            await _context.Posts.AddAsync(post);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<string>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the video");
            }
            return Result<string>.CreateSuccessful(post.Id.ToString());
        }

        public async Task<Result<string>> AddPostMetadata(string userId, string title, string postId)
        {
            var user = await _context.Users.Include(t => t.Photo).SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<string>.CreateFailed(
                    HttpStatusCode.NotFound, "User Does Not Exit");
            }

            Post post = await _context.Posts.FindAsync(Guid.Parse(postId));
            if (post == null)
            {
                return Result<string>.CreateFailed(
                    HttpStatusCode.NotFound, "Post Does Not Exit");
            }
            post.Title = title;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<string>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the video");
            }
            return Result<string>.CreateSuccessful(post.Id.ToString());
        }


        public async Task<Result<PostDTO>> GetPost(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<PostDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var post = await _context.Posts
                .Include(x=>x.Owner)
                .Include(x=>x.Subscribers)
                .SingleOrDefaultAsync(x => x.Owner.Id == Guid.Parse(userId));

            if (post == null)
            {
                return Result<PostDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't get the post");
            }

            return Result<PostDTO>.CreateSuccessful(new PostDTO(post, post.Owner, post.Subscribers));
        }

        public async Task<Result<IEnumerable<PostDTO>>> GetPosts(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<IEnumerable<PostDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var posts = await _context.Posts
                .Include(x => x.Owner)
                .Include(x => x.Subscribers).ToListAsync();

            if (posts == null)
            {
                return Result<IEnumerable<PostDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't get the post");
            }

            return Result<IEnumerable<PostDTO>>.CreateSuccessful(PostDTO.ToPostDTOList(posts));
        }
    }
}
