using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.PersonalSkills;

namespace Ted.Model.DTO
{
    public class SkillsDTO
    {
        public IEnumerable<ExperienceDTO> Experiences { get; set; }
        public IEnumerable<EducationDTO> Educations { get; set; }
        public IEnumerable<PersonalSkillDTO> PersonalSkills { get; set; }

        public SkillsDTO(IEnumerable<ExperienceDTO> experiences, IEnumerable<EducationDTO> educations, IEnumerable<PersonalSkillDTO> personalSkills)
        {
            Experiences = experiences;
            Educations = educations;
            PersonalSkills = personalSkills;
        }
    }
}
