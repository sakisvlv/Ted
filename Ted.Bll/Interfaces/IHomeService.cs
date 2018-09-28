using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model;
using Ted.Model.DTO;
using Ted.Model.Posts;

namespace Ted.Bll.Interfaces
{
    public interface IHomeService
    {
        Task<Result<ExperienceDTO>> GetLastExperience(string userId);
        Task<Result<Guid>> InsertImage(string userId, IFormFile file, string format);
        Task<Result<Guid>> InsertAudio(string userId, IFormFile file, string format);
        Task<Result<Guid>> InsertVideo(string userId, IFormFile file, string format);
        Task<Result<PostDTO>> AddPostMetadata(string userId, string title, string postId);
        Task<Result<PostDTO>> GetPost(string userId, string postId);
        Task<Result<IEnumerable<PostDTO>>> GetPosts(string userId, int page);
        Task<Result<int>> GetConnectionsCount(string userId);
        Task<Result<PostDTO>> InsertArticle(string userId, string content, string description);
        Task<Result<bool>> DeletePost(string userId, string id);
        Task<Result<UserInfoSmallDTO>> SubscribeToPost(string userId, string id);
        Task<Result<PostDTO>> UpdatePost(string userId, PostDTO postDTO);
        Task<Result<bool>> UnsubscribeFromPost(string userId, string id);
        Task<Result<bool>> DeleteComment(string userId, string commnetId);
        Task<Result<CommentDTO>> PostComment(string userId, string postId, CommentDTO commentDTO);
        Task<Result<byte[]>> GetPhoto(string userId, string userToGetId);
        Task<Result<string>> GetContent(string userId, string contentId);
    }
}
