using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ted.Model.Auth;
using Ted.Model.PersonalSkills;
using Ted.Model.Posts;

namespace Ted.Model.DTO
{
    public class PostDTO
    {
        public string Title { get; set; }
        public UserInfoSmallDTO User { get; set; }
        public IEnumerable<UserInfoSmallDTO> Subscribers { get; set; }
        public PostType Type { get; set; }
        public byte[] Content { get; set; }
        public DateTime PostedDate { get; set; }

        public PostDTO()
        {
        }

        public PostDTO(Post post, User user, List<User> subscribers)
        {
            Title = post.Title;
            User = new UserInfoSmallDTO(user);
            Subscribers = UserInfoSmallDTO.ToPersonalSkillDTOList(subscribers);
            Type = post.Type;
            Content = post.Content;
            PostedDate = post.PostedDate;
        }

        public static IEnumerable<PostDTO> ToPostDTOList(List<Post> posts)
        {
            return posts.Select(x => new PostDTO(x, x.Owner, x.Subscribers));
        }
    }
}
