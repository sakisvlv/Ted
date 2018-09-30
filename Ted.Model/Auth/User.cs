using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using Ted.Model.Ads;
using Ted.Model.Network;
using Ted.Model.Notifications;
using Ted.Model.PersonalSkills;
using Ted.Model.Posts;

namespace Ted.Model.Auth
{
    public class User : IdentityUser<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public override Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CurrentState { get; set; }
        public Photo Photo { get; set; }
        [XmlIgnore]
        public IList<UserPost> UserPosts { get; set; }
        [XmlIgnore]
        public IList<UserAd> UserAds { get; set; }
        [XmlIgnore]
        public virtual ICollection<Friend> FriendTo { get; set; }
        [XmlIgnore]
        public virtual ICollection<Friend> FrendFrom { get; set; }
        [XmlIgnore]
        public ICollection<Experience> Experiences { get; set; }
        [XmlIgnore]
        public ICollection<Education> Educations { get; set; }
        [XmlIgnore]
        public ICollection<PersonalSkill> PersonalSkills { get; set; }
        [XmlIgnore]
        public ICollection<Notification> Notifications { get; set; }
        [XmlIgnore]
        public ICollection<SkillKnn> SkillKnns { get; set; }
        [XmlIgnore]
        public ICollection<PostKnn> PostKnns { get; set; }

        public string ToXML()
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(stringwriter, this);
            return stringwriter.ToString();
        }
    }
}
