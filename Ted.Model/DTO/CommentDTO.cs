using System;
using System.Collections.Generic;
using System.Text;

namespace Ted.Model.DTO
{
   public class CommentDTO
    {
        public UserInfoSmallDTO User { get; set; }
        public string Text { get; set; }
    }
}
