using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ted.Model.PersonalSkills;

namespace Ted.Model.DTO
{
    public class EducationDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string Degree { get; set; }
        public string Field { get; set; }
        public float Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Link { get; set; }
        public bool StillThere { get; set; }

        public EducationDTO()
        {
        }

        public EducationDTO(Education education)
        {
            Id = education.Id;
            Title = education.Title;
            Description = education.Description;
            IsPublic = education.IsPublic;
            Degree = education.Degree;
            Field = education.Field;
            Grade = education.Grade;
            StartDate = education.StartDate;
            EndDate = education.EndDate;
            Link = education.Link;
            StillThere = education.StillThere;
        }
        public static IEnumerable<EducationDTO> ToEducationDTOList(List<Education> educations)
        {
            return educations.Select(x => new EducationDTO(x));
        }
    }
}
