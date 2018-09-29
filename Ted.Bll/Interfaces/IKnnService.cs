using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model.Ads;
using Ted.Model.Auth;
using Ted.Model.DTO;
using Ted.Model.PersonalSkills;

namespace Ted.Bll.Interfaces
{
    public interface IKnnService
    {
        Task ManageAd(AdDTO adDTO);
        Task ManageSkill(Skill skill, User user);
        Task RemoveAd(Ad ad);
        Task RemoveSkill(Skill skill, User user);
        double GetDistance(Ad ad, User user);
    }
}
