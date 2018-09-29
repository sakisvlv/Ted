using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ted.Model.Ads;

namespace Ted.Model.DTO
{
    public class AdDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }

        public AdDTO(Ad ad)
        {
            Title = ad.Title;
            Description = ad.Description;
            Company = ad.Company;
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
