using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ted.Model
{
    public class XMLS
    {
        public List<XML> XMLs { get; set; }
        public string ToXML()
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(stringwriter, this);
            return stringwriter.ToString();
        }
    }
}
