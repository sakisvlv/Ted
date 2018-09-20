using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ted.Model.Posts;

namespace Ted.Model.DTO
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public UserInfoSmallDTO User { get; set; }
        public string Text { get; set; }
        public DateTime CommentDate { get; set; }

        public CommentDTO(Comment comment)
        {
            Id = comment.Id;
            User = new UserInfoSmallDTO(comment.User);
            Text = comment.Text;
            CommentDate = comment.CommentDate;
        }

        public CommentDTO()
        {
        }

        public static IEnumerable<CommentDTO> ToCommentDTOList(ICollection<Comment> comments)
        {
            return comments?.Select(x => new CommentDTO(x));
        }
    }
}
