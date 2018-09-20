using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ted.Model.Auth;
using Ted.Model.PersonalSkills;

namespace Ted.Model.DTO
{
    public class UserInfoSmallDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CurrentState { get; set; }

        public UserInfoSmallDTO(User user)
        {
            Id = user.Id.ToString();
            FirstName = user.FirstName;
            LastName = user.LastName;
            CurrentState = user.CurrentState;
        }
        public UserInfoSmallDTO()
        {
        }

        public static IEnumerable<UserInfoSmallDTO> ToUserInfoSmallDTOList(IEnumerable<User> users)
        {
            return users?.Select(x => new UserInfoSmallDTO(x));
        }
    }
}
