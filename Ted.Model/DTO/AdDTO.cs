using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ted.Model.Ads;
using Ted.Model.Auth;

namespace Ted.Model.DTO
{
    public class AdDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
        public UserInfoSmallDTO Owner { get; set; }
        public IEnumerable<UserInfoSmallDTO> Applicants { get; set; }

        public AdDTO(Ad ad)
        {
            Id = ad.Id.ToString();
            Title = ad.Title;
            Description = ad.Description;
            Company = ad.Company;
            Owner = new UserInfoSmallDTO(ad.Owner);
            Applicants = UserInfoSmallDTO.ToUserInfoSmallDTOList(ad.UserAds?.Select(x=>x.User));
        }

        public AdDTO()
        {
        }

        public static IEnumerable<AdDTO> ToAdDTOList(IEnumerable<Ad> ads)
        {
            return ads.Select(x => new AdDTO(x));
        }
    }
}
