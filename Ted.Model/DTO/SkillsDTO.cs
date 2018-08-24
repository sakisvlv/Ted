using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.PersonalSkills;

namespace Ted.Model.DTO
{
    public class SkillsDTO
    {
        public List<Experience> Experiences { get; set; }
        public List<Education> Educations { get; set; }
        public List<PersonalSkill> PersonalSkills { get; set; }

        public SkillsDTO(List<Experience> experiences, List<Education> educations, List<PersonalSkill> personalSkills)
        {
            Experiences = experiences;
            Educations = educations;
            PersonalSkills = personalSkills;
        }
    }
}
