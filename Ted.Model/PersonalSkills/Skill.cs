using System;
using System.Collections.Generic;
using System.Text;

namespace Ted.Model.PersonalSkills
{
    public abstract class Skill : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }

    }
}
