using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.DTO
{
    public class UserListItemDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public UserListItemDTO()
        {
        }

        public UserListItemDTO(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
        }

        public static IEnumerable<UserListItemDTO> ToDeviceDTOList(IEnumerable<User> items)
        {
            return items.Select(x => new UserListItemDTO(x));
        }
    }
}