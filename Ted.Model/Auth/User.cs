﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ted.Model.Network;
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
        public IList<UserPost> UserPosts { get; set; }
        public virtual ICollection<Friend> FriendTo { get; set; }
        public virtual ICollection<Friend> FrendFrom { get; set; }
        public ICollection<Experience> Experiences { get; set; }
        public ICollection<Education> Educations { get; set; }
        public ICollection<PersonalSkill> PersonalSkills { get; set; }
    }
}
