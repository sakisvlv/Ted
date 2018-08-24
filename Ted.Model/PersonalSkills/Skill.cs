using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.PersonalSkills
{
    public class Skill : Entity
    {
        public User User { set; get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
}
