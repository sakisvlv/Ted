using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model;
using Ted.Model.DTO;

namespace Ted.Bll.Interfaces
{
    public interface IHomeService
    {
        Task<Result<ExperienceDTO>> GetLastExperience(string userId);
        Task<Result<string>> InsertImage(string userId, byte[] imageByteArray);
        Task<Result<string>> InsertVideo(string userId, byte[] videoByteArray);
        Task<Result<string>> AddPostMetadata(string userId, string title, string postId);
        Task<Result<PostDTO>> GetPost(string userId);
        Task<Result<IEnumerable<PostDTO>>> GetPosts(string userId, int page);
        Task<Result<int>> GetConnectionsCount(string userId);
        Task<Result<PostDTO>> InsertArticle(string userId, string content);
        Task<Result<bool>> DeletePost(string userId, string id);
        Task<Result<bool>> SubscribeToPost(string userId, string id);
        Task<Result<PostDTO>> UpdatePost(string userId, PostDTO postDTO);
        Task<Result<bool>> UnsubscribeFromPost(string userId, string id);
    }
}
