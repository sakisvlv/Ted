using System;
using System.Collections.Generic;
using System.Text;

namespace Ted.Model.DTO
{
    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
