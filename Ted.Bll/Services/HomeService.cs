using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ted.Bll.Interfaces;
using Ted.Dal;
using Ted.Knn;
using Ted.Model;
using Ted.Model.Auth;
using Ted.Model.DTO;
using Ted.Model.Notifications;
using Ted.Model.Posts;

namespace Ted.Bll.Services
{
    public class HomeService : IHomeService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly string _filePath;
        private readonly IKnnService _knnService;

        public HomeService(IConfiguration configuration, Context context, UserManager<User> userManager, IKnnService knnService)
        {
            _filePath = configuration.GetSection("filePath").Value;
            _context = context;
            _userManager = userManager;
            _knnService = knnService;
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

        public async Task<Result<PostDTO>> InsertArticle(string userId, string content, string description)
        {
            var user = await _context.Users.Include(t => t.Photo).SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<PostDTO>.CreateFailed(
                    HttpStatusCode.NotFound, "User Does Not Exit");
            }

            var post = new Post();
            post.Owner = user;
            post.Description = description;
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

        public async Task<Result<Guid>> InsertImage(string userId, IFormFile file, string format)
        {
            var user = await _context.Users.Include(t => t.Photo).SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<Guid>.CreateFailed(
                    HttpStatusCode.NotFound, "User Does Not Exit");
            }

            Post post = new Post(user, PostType.Image, Guid.NewGuid().ToString() + format, DateTime.Now);
            await _context.Posts.AddAsync(post);

            var filePath = Path.Combine(_filePath, post.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<Guid>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the photo");
            }
            return Result<Guid>.CreateSuccessful(post.Id);
        }

        public async Task<Result<Guid>> InsertAudio(string userId, IFormFile file, string format)
        {
            var user = await _context.Users.Include(t => t.Photo).SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<Guid>.CreateFailed(
                    HttpStatusCode.NotFound, "User Does Not Exit");
            }
            Post post = new Post(user, PostType.Audio, Guid.NewGuid().ToString() + format, DateTime.Now);
            await _context.Posts.AddAsync(post);

            var filePath = Path.Combine(_filePath, post.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<Guid>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the photo");
            }
            return Result<Guid>.CreateSuccessful(post.Id);
        }

        public async Task<Result<Guid>> InsertVideo(string userId, IFormFile file, string format)
        {
            var user = await _context.Users.Include(t => t.Photo).SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<Guid>.CreateFailed(
                    HttpStatusCode.NotFound, "User Does Not Exit");
            }

            Post post = new Post(user, PostType.Video, Guid.NewGuid().ToString() + format, DateTime.Now);
            await _context.Posts.AddAsync(post);

            var filePath = Path.Combine(_filePath, post.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<Guid>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the video");
            }
            return Result<Guid>.CreateSuccessful(post.Id);
        }

        public async Task<Result<PostDTO>> AddPostMetadata(string userId, string title, string postId)
        {
            var user = await _context.Users.Include(t => t.Photo).SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<PostDTO>.CreateFailed(
                    HttpStatusCode.NotFound, "User Does Not Exit");
            }

            Post post = await _context.Posts
                .Where(x => x.Id == Guid.Parse(postId))
                .Include(x => x.Owner).FirstOrDefaultAsync();

            if (post == null)
            {
                return Result<PostDTO>.CreateFailed(
                    HttpStatusCode.NotFound, "Post Does Not Exit");
            }
            post.Title = title;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<PostDTO>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the video");
            }
            return Result<PostDTO>.CreateSuccessful(new PostDTO(post));
        }

        public async Task<Result<PostDTO>> GetPost(string userId, string postId)
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
                .Include(x => x.Comments)
                .Where(x => x.Id == Guid.Parse(postId))
                .FirstOrDefaultAsync();

            if (post == null)
            {
                return Result<PostDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't get the post");
            }

            return Result<PostDTO>.CreateSuccessful(new PostDTO(post));
        }

        public async Task<Result<IEnumerable<PostDTO>>> GetPosts(string userId, int page)
        {
            var user = await _context.Users.Where(x => x.Id == Guid.Parse(userId))
                .Include(x => x.PostKnns)
                .Include("PostKnns.GlobalString")
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return Result<IEnumerable<PostDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            var posts = new List<Post>();

            var friendlist = await _context.Friends
                .Where(x => (x.FromUser == user || x.ToUser == user) && (x.FromUserAccepted && x.ToUserAccepted))
                .Include(x => x.FromUser)
                .Include("FromUser.UserPosts")
                .Include(x => x.ToUser)
                .Include("ToUser.UserPosts")
                .ToListAsync();

            if (friendlist == null)
            {
                return Result<IEnumerable<PostDTO>>.CreateFailed(
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

            var friendsIds = friends.Select(x => x.Id);

            var postIds = new List<Guid>();
            foreach (var fr in friends)
            {
                postIds.AddRange(fr.UserPosts.Select(x => x.PostId).ToList());
            }



            posts = await _context.Posts
            .Include(x => x.Owner)
            .Include(x => x.UserPosts)
            .Include(x => x.Pknns)
            .Include("Pknns.GlobalString")
            .Include("UserPosts.User")
            .Include(x => x.Comments)
            .Where(x => x.Owner.Id == user.Id || friendsIds.Contains(x.Owner.Id) || postIds.Contains(x.Id))
            .Distinct()
            .OrderByDescending(x => x.PostedDate)
            .Skip(page * 10)
            .Take(10)
            .ToListAsync();


            if (posts == null)
            {
                return Result<IEnumerable<PostDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't get the post");
            }

            if (posts.Count == 0)
            {
                return Result<IEnumerable<PostDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "There is no more posts");
            }

            //double distance;
            //var topDistances = new List<Tuple<double, Post>>();

            //foreach (var post in posts)
            //{
            //    distance = _knnService.GetDistance(post, user);
            //    if (topDistances.Count < 10)
            //    {
            //        topDistances.Add(new Tuple<double, Post>(distance, post));
            //    }
            //    else
            //    {
            //        var max = topDistances.Max(x => x.Item1);
            //        var maxItem = topDistances.Where(x => x.Item1 == max).FirstOrDefault();
            //        if (max > distance)
            //        {
            //            topDistances[topDistances.IndexOf(maxItem)] = new Tuple<double, Post>(distance, post);
            //        }
            //    }
            //}

            //var result = topDistances.Select(x => x.Item2);

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

            if (post.FileName != null)
            {
                var filePath = Path.Combine(_filePath, post.FileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
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

        public async Task<Result<UserInfoSmallDTO>> SubscribeToPost(string userId, string id)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<UserInfoSmallDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var post = await _context.Posts
                .Where(x => x.Id == Guid.Parse(id))
                .Include(x => x.Owner)
                .Include(x => x.UserPosts)
                .Include(x => x.Comments)
                .Include("UserPosts.User")
                .FirstOrDefaultAsync();
            if (post == null)
            {
                return Result<UserInfoSmallDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't delete the post");
            }

            var userPost = new UserPost();
            userPost.Post = post;
            userPost.User = user;
            var posts = _context.UserPost;
            posts.Add(userPost);

            var subscribers = post.UserPosts.Select(x => x.User);
            var comentants = post.Comments.Select(x => x.User);
            var intersect = subscribers.Union(comentants);

            foreach (var subscriber in intersect)
            {
                if (subscriber != user)
                {
                    var notification = new Notification
                    {
                        IsAcknowledged = false,
                        PostId = post.Id,
                        Sender = user.FirstName + " " + user.LastName,
                        SenderId = user.Id,
                        ToUser = subscriber,
                        Type = NotificationType.Subscribe,
                        DateAdded = DateTime.Now
                    };
                    _context.Notifications.Add(notification);
                }
            }

            if (post.Owner != user)
            {
                var notification = new Notification
                {
                    IsAcknowledged = false,
                    PostId = post.Id,
                    Sender = user.FirstName + " " + user.LastName,
                    SenderId = user.Id,
                    ToUser = post.Owner,
                    Type = NotificationType.Subscribe,
                    DateAdded = DateTime.Now
                };
                _context.Notifications.Add(notification);
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<UserInfoSmallDTO>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't save the video");
            }

            return Result<UserInfoSmallDTO>.CreateSuccessful(new UserInfoSmallDTO(user));
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

        public async Task<Result<CommentDTO>> PostComment(string userId, string postId, CommentDTO commentDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<CommentDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var post = await _context.Posts
                .Where(x => x.Id == Guid.Parse(postId))
                .Include(x => x.Owner)
                .Include(x => x.UserPosts)
                .Include(x => x.Comments)
                .Include("UserPosts.User")
                .FirstOrDefaultAsync();

            if (post == null)
            {
                return Result<CommentDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't post comment");
            }

            var comment = new Comment();
            comment.Text = commentDTO.Text;
            comment.User = user;
            post.Comments.Add(comment);


            var subscribers = post.UserPosts.Select(x => x.User);
            var comentants = post.Comments.Select(x => x.User);
            var intersect = subscribers.Union(comentants);

            foreach (var subscriber in intersect)
            {
                if (subscriber != user)
                {
                    var notification = new Notification
                    {
                        IsAcknowledged = false,
                        PostId = post.Id,
                        Sender = user.FirstName + " " + user.LastName,
                        SenderId = user.Id,
                        ToUser = subscriber,
                        Type = NotificationType.Comment,
                        DateAdded = DateTime.Now
                    };
                    _context.Notifications.Add(notification);
                }
            }

            if (post.Owner != user)
            {
                var notification = new Notification
                {
                    IsAcknowledged = false,
                    PostId = post.Id,
                    Sender = user.FirstName + " " + user.LastName,
                    SenderId = user.Id,
                    ToUser = post.Owner,
                    Type = NotificationType.Comment,
                    DateAdded = DateTime.Now
                };
                _context.Notifications.Add(notification);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<CommentDTO>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't post comment");
            }

            return Result<CommentDTO>.CreateSuccessful(new CommentDTO(comment));
        }

        public async Task<Result<bool>> DeleteComment(string userId, string commnetId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var comment = await _context.Comments
                .Where(x => x.Id == Guid.Parse(commnetId))
                .Include(x => x.User)
                .FirstOrDefaultAsync();
            if (comment == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't delete the comment");
            }

            if (comment.User != user)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "Cound't delete the comment");
            }

            _context.Comments.Remove(comment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't delete the comment");
            }

            return Result<bool>.CreateSuccessful(true);
        }

        public async Task<Result<byte[]>> GetPhoto(string userId, string userToGetId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<byte[]>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var userToGet = await _userManager.FindByIdAsync(userToGetId);
            if (user == null)
            {
                return Result<byte[]>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var photo = await _context.Photos.SingleOrDefaultAsync(x => x.UserId == Guid.Parse(userToGetId));
            if (photo == null)
            {
                return Result<byte[]>.CreateSuccessful(null);
            }
            return Result<byte[]>.CreateSuccessful(photo.File);
        }

        public async Task<Result<string>> GetContent(string userId, string contentId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<string>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            if (user == null)
            {
                return Result<string>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }
            return Result<string>.CreateSuccessful("");
        }

    }
}
