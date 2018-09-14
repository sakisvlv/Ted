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
    }
}
