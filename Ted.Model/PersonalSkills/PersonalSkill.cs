using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Ted.Model.Auth;
using Ted.Model.DTO;

namespace Ted.Model.PersonalSkills
{
    public class PersonalSkill : Skill
    {
        public void Update(PersonalSkillDTO personalSkill)
        {
            Title = personalSkill.Title;
            Description = personalSkill.Description;
            IsPublic = personalSkill.IsPublic;
        }
    }
}
