using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ted.Model.PersonalSkills;

namespace Ted.Model.DTO
{
    public class PersonalSkillDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public PersonalSkillDTO()
        {
        }

        public PersonalSkillDTO(PersonalSkill personalSkill)
        {
            Title = personalSkill.Title;
            Description = personalSkill.Description;
            IsPublic = personalSkill.IsPublic;
            Id = personalSkill.Id;
        }
        public static IEnumerable<PersonalSkillDTO> ToPersonalSkillDTOList(List<PersonalSkill> personalSkill)
        {
            return personalSkill.Select(x => new PersonalSkillDTO(x));
        }
    }
}
