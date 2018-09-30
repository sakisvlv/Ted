using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Serialization;
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

        public string ToXML()
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(stringwriter, this);
            return stringwriter.ToString();
        }
    }
}
