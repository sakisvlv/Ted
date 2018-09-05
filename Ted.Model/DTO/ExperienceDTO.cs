using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ted.Model.PersonalSkills;

namespace Ted.Model.DTO
{
    public class ExperienceDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string Company { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Link { get; set; }
        public bool StillThere { get; set; }

        public ExperienceDTO()
        {
        }

        public ExperienceDTO(Experience experience)
        {
            Id = experience.Id;
            Title = experience.Title;
            Description = experience.Description;
            IsPublic = experience.IsPublic;
            Company = experience.Company;
            StartDate = experience.StartDate;
            EndDate = experience.EndDate;
            Link = experience.Link;
            StillThere = experience.StillThere;
        }

        public static IEnumerable<ExperienceDTO> ToExperienceDTOList(List<Experience> experiences)
        {
            return experiences.Select(x => new ExperienceDTO(x));
        }
    }
}
