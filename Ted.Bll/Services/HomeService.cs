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

        public async Task<Result<PostDTO>> InsertArticle(string userId, string content)
        {
            var user = await _context.Users.Include(t => t.Photo).SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<PostDTO>.CreateFailed(
                    HttpStatusCode.NotFound, "User Does Not Exit");
            }

            var post = new Post();
            post.Owner = user;
            post.Title = content;
            post.PostedDate = DateTime.Now;
            post.Type = PostType.Article;

            await _context.Posts.AddAsync(post);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<PostDTO>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the photo");
            }
            return Result<PostDTO>.CreateSuccessful(new PostDTO(post));
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
                .Include(x => x.Owner)
                .Include(x => x.UserPosts)
                .Include("UserPosts.User")
                .SingleOrDefaultAsync(x => x.Owner.Id == Guid.Parse(userId));

            if (post == null)
            {
                return Result<PostDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't get the post");
            }

            return Result<PostDTO>.CreateSuccessful(new PostDTO(post));
        }

        public async Task<Result<IEnumerable<PostDTO>>> GetPosts(string userId, int page)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<IEnumerable<PostDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            var posts = new List<Post>();

            posts = await _context.Posts
            .Include(x => x.Owner)
            .Include(x => x.UserPosts)
            .Include("UserPosts.User")
            .Include(x => x.Comments)
            .Where(x => x.Owner.Id == user.Id)
            .OrderByDescending(x => x.PostedDate)
            .Skip(page * 10)
            .Take(10)
            .ToListAsync();


            if (posts == null)
            {
                return Result<IEnumerable<PostDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't get the post");
            }

            return Result<IEnumerable<PostDTO>>.CreateSuccessful(PostDTO.ToPostDTOList(posts));
        }

        public async Task<Result<int>> GetConnectionsCount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<int>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            var friendlist = await _context.Friends
                .Where(x => (x.FromUser == user || x.ToUser == user) && (x.FromUserAccepted && x.ToUserAccepted))
                .Include(x => x.FromUser)
                .Include(x => x.ToUser)
                .ToListAsync();

            if (friendlist == null)
            {
                return Result<int>.CreateFailed(
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

            return Result<int>.CreateSuccessful(friends.Count);
        }

        public async Task<Result<bool>> DeletePost(string userId, string id)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var post = await _context.Posts.Where(x => x.Id == Guid.Parse(id)).FirstOrDefaultAsync();
            if (post == null || post.Owner != user)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't delete the post");
            }

            _context.Posts.Remove(post);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the video");
            }

            return Result<bool>.CreateSuccessful(true);
        }

        public async Task<Result<bool>> SubscribeToPost(string userId, string id)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var post = await _context.Posts.Where(x => x.Id == Guid.Parse(id)).FirstOrDefaultAsync();
            if (post == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't delete the post");
            }

            var userPost = new UserPost();
            userPost.Post = post;
            userPost.User = user;
            post.UserPosts.Add(userPost);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the video");
            }

            return Result<bool>.CreateSuccessful(true);
        }

        public async Task<Result<bool>> UnsubscribeFromPost(string userId, string id)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var post = await _context.Posts.Where(x => x.Id == Guid.Parse(id)).FirstOrDefaultAsync();
            if (post == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't delete the post");
            }

            var relation = _context.UserPost.Where(x => x.Post == post && x.User == user);

            if (relation == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't unsubscribe the post");
            }

            _context.UserPost.Remove(relation.FirstOrDefault());

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the video");
            }

            return Result<bool>.CreateSuccessful(true);
        }

        public async Task<Result<PostDTO>> UpdatePost(string userId, PostDTO postDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<PostDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var post = await _context.Posts.Where(x => x.Id == postDTO.Id).FirstOrDefaultAsync();
            if (post == null)
            {
                return Result<PostDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't delete the post");
            }

            post.Title = postDTO.Title;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<PostDTO>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the video");
            }

            return Result<PostDTO>.CreateSuccessful(postDTO);
        }
    }
}
