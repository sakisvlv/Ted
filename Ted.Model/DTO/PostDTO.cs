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
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserInfoSmallDTO User { get; set; }
        public IEnumerable<UserInfoSmallDTO> Subscribers { get; set; }
        public PostType Type { get; set; }
        public string FileName { get; set; }
        public DateTime PostedDate { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }

        public PostDTO()
        {
        }

        public PostDTO(Post post)
        {
            Id = post.Id;
            Title = post.Title;
            FileName = post.FileName;
            User = new UserInfoSmallDTO(post.Owner);
            Subscribers = UserInfoSmallDTO.ToUserInfoSmallDTOList(post.UserPosts?.Select(x => x.User));
            Comments = CommentDTO.ToCommentDTOList(post.Comments);
            Type = post.Type;
            PostedDate = post.PostedDate;
            Description = post.Description;
        }

        public static IEnumerable<PostDTO> ToPostDTOList(List<Post> posts)
        {
            return posts.Select(x => new PostDTO(x));
        }
    }
}
