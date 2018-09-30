using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Ted.Model.Ads;
using Ted.Model.Auth;
using Ted.Model.PersonalSkills;
using Ted.Model.Posts;

namespace Ted.Model
{
    public class XML
    {
        public User User { get; set; }
        public List<User> Friends { set; get; }
        public List<Comment> Comments { set; get; }
        public List<Experience> Expiriances { set; get; }
        public List<Education> Educations { set; get; }
        public List<PersonalSkill> Personalskills { set; get; }
        public List<Post> Posts { set; get; }
        public List<Ad> Ads { set; get; }


        public string ToXML()
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(stringwriter, this);
            return stringwriter.ToString();
        }
    }
}
