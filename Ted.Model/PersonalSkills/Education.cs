using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Model.PersonalSkills
{
    public class Education : Skill
    {
        
        public string Degree { get; set; }
        public string Feild { get; set; }
        public float Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Link { get; set; }
        public bool StillThere { get; set; }
    }
}
