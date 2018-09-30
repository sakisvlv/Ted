using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model;
using Ted.Model.DTO;

namespace Ted.Bll.Interfaces
{
    public interface IAdService
    {
        Task<Result<bool>> AddAd(string userId, AdDTO adDTO);
        Task<Result<IEnumerable<AdDTO>>> GetAds(string userId);
        Task<Result<IEnumerable<AdDTO>>> GetMyAds(string userId);
        Task<Result<bool>> ApplyToAd(string userId, string adId);
        Task<Result<bool>> DeleteAd(string userId, string adId);
    }
}
