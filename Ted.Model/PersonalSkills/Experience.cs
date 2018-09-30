using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Serialization;
using Ted.Model.Auth;
using Ted.Model.DTO;

namespace Ted.Model.PersonalSkills
{
    public class Experience : Skill
    {
        public string Company { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Link { get; set; }
        public bool StillThere { get; set; }

        public void Update(ExperienceDTO experienceDTO)
        {
            Company = experienceDTO.Company;
            StartDate = experienceDTO.StartDate;
            EndDate = experienceDTO.EndDate;
            Link = experienceDTO.Link;
            StillThere = experienceDTO.StillThere;
            Title = experienceDTO.Title;
            Description = experienceDTO.Description;
            IsPublic = experienceDTO.IsPublic;
        }

        public string ToXML()
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(stringwriter, this);
            return stringwriter.ToString();
        }
    }
}
