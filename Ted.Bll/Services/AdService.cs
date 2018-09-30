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
using Ted.Knn;
using Ted.Model;
using Ted.Model.Ads;
using Ted.Model.Auth;
using Ted.Model.DTO;

namespace Ted.Bll.Services
{
    public class AdService : IAdService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IKnnService _knnService;

        public AdService(Context context, UserManager<User> userManager, IKnnService knnService)
        {
            _context = context;
            _userManager = userManager;
            _knnService = knnService;
        }

        public async Task<Result<bool>> AddAd(string userId, AdDTO adDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            await _knnService.ManageAd(adDTO);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't add ad");
            }

            return Result<bool>.CreateSuccessful(true);
        }

        public async Task<Result<IEnumerable<AdDTO>>> GetAds(string userId)
        {
            var user = await _context.Users.Where(x => x.Id == Guid.Parse(userId))
                .Include(x => x.SkillKnns)
                .Include("SkillKnns.GlobalString")
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Result<IEnumerable<AdDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var ads = await _context.Ads
                .Where(x => x.Owner != user && !x.UserAds.Select(y => y.User).Contains(user))
                .Include(x => x.UserAds)
                .Include("UserAds.User")
                .Include(x => x.AdKnns)
                .Include(x => x.Owner)
                .Include("AdKnns.GlobalString")
                .ToListAsync();

            double distance;
            var topDistances = new List<Tuple<double, Ad>>();

            foreach (var ad in ads)
            {
                distance = _knnService.GetDistance(ad, user);
                if (topDistances.Count < 10)
                {
                    topDistances.Add(new Tuple<double, Ad>(distance, ad));
                }
                else
                {
                    var max = topDistances.Max(x => x.Item1);
                    var maxItem = topDistances.Where(x => x.Item1 == max).FirstOrDefault();
                    if (max > distance)
                    {
                        topDistances[topDistances.IndexOf(maxItem)] = new Tuple<double, Ad>(distance, ad);
                    }
                }
            }

            var result = topDistances.Select(x => x.Item2);

            return Result<IEnumerable<AdDTO>>.CreateSuccessful(AdDTO.ToAdDTOList(result));
        }

        public async Task<Result<IEnumerable<AdDTO>>> GetMyAds(string userId)
        {
            var user = await _context.Users.Where(x => x.Id == Guid.Parse(userId))
                .Include(x => x.SkillKnns)
                .Include("SkillKnns.GlobalString")
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Result<IEnumerable<AdDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var ads = await _context.Ads
                .Where(x => x.Owner == user)
                .Include(x => x.UserAds)
                .Include("UserAds.User")
                .Include(x => x.AdKnns)
                .Include(x => x.Owner)
                .Include("AdKnns.GlobalString")
                .ToListAsync();

            return Result<IEnumerable<AdDTO>>.CreateSuccessful(AdDTO.ToAdDTOList(ads));
        }

        public async Task<Result<bool>> ApplyToAd(string userId, string adId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var ad = await _context.Ads
                .Where(x => x.Id == Guid.Parse(adId))
                .Include(x => x.UserAds)
                .Include("UserAds.User")
                .Include(x => x.Owner)
                .FirstOrDefaultAsync();

            var applicant = new UserAd();
            applicant.User = user;
            applicant.Ad = ad;
            ad.UserAds.Add(applicant);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't add ad");
            }

            return Result<bool>.CreateSuccessful(true);
        }

        public async Task<Result<bool>> DeleteAd(string userId, string adId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var ad = await _context.Ads
                .Where(x => x.Id == Guid.Parse(adId))
                .Include(x => x.UserAds)
                .Include("UserAds.User")
                .Include(x => x.Owner)
                .FirstOrDefaultAsync();

            var toDelete =  _context.AdKnns.Where(x=>x.Ad == ad);
            _context.AdKnns.RemoveRange(toDelete);
            _context.Ads.Remove(ad);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't delete ad");
            }

            return Result<bool>.CreateSuccessful(true);
        }

    }
}
