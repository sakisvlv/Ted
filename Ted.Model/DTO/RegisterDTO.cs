using System;
using System.Collections.Generic;
using System.Text;

namespace Ted.Model.DTO
{
    public class RegisterDTO
    {
        public string Email { set; get; }
        public string Password { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string PhoneNumber { set; get; }
    }
}
