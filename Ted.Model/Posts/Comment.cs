using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Ted.Model.Auth;

namespace Ted.Model.Posts
{
    public class Comment : Entity
    {
        public User User { get; set; }
        public string Text { get; set; }
        public DateTime CommentDate { get; set; }


        public string ToXML()
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(this.GetType());
            serializer.Serialize(stringwriter, this);
            return stringwriter.ToString();
        }
    }
}
