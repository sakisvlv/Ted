using System;
using System.Collections.Generic;
using System.Text;

namespace Ted.Model.PersonalSkills
{
    public class Education : Skill
    {
        public string Degree { get; set; }
        public string Feild { get; set; }
        public string Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Link { get; set; }
    }
}
