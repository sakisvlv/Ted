using System;

namespace Ted.Model
{
    public class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Photo { get; set; }
        public Credentials Credentials { get; set; }
    }
}
