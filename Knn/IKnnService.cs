using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model.Ads;
using Ted.Model.Auth;
using Ted.Model.DTO;
using Ted.Model.PersonalSkills;
using Ted.Model.Posts;

namespace Ted.Knn
{
    public interface IKnnService
    {
        Task ManageAd(AdDTO adDTO);
        Task ManageSkill(Skill skill, User user);
        Task RemoveAd(Ad ad);
        Task RemoveSkill(Skill skill, User user);
        Task ManageHistory(string text, User user);
        Task ManagePosts(string text, Post post);
        double GetDistance(Ad ad, User user);
        double GetDistance(Post post, User user);
    }
}
