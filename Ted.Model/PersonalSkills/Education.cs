using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Serialization;
using Ted.Model.Auth;
using Ted.Model.DTO;

namespace Ted.Model.PersonalSkills
{
    public class Education : Skill
    {
        public string Degree { get; set; }
        public string Field { get; set; }
        public float Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Link { get; set; }
        public bool StillThere { get; set; }
        public void Update(EducationDTO education)
        {
            Degree = education.Degree;
            Field = education.Field;
            Grade = education.Grade;
            StartDate = education.StartDate;
            EndDate = education.EndDate;
            Link = education.Link;
            StillThere = education.StillThere;
            Title = education.Title;
            Description = education.Description;
            IsPublic = education.IsPublic;
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
