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
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<IEnumerable<AdDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var ads = await _context.Ads
                .Include(x => x.AdKnns)
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
                    var min = topDistances.Min(x => x.Item1);
                    var minItem = topDistances.Where(x=>x.Item1 == min).FirstOrDefault();
                    if (min < distance)
                    {
                        topDistances[topDistances.IndexOf(minItem)] = new Tuple<double, Ad>(distance, ad);
                    }
                }
            }

            var result = topDistances.Select(x=>x.Item2);

            return Result<IEnumerable<AdDTO>>.CreateSuccessful(AdDTO.ToAdDTOList(result));
        }
    }
}
