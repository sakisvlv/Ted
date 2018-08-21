using System;
using System.Collections.Generic;
using System.Text;

namespace Ted.Model.DTO
{
    public class CountsDTO
    {
        public int Users { get; set; }
        public int Posts { get; set; }
        public int Ads { get; set; }
        public int Notifications { get; set; }

        public CountsDTO()
        {
            Users = 0;
            Posts = 0;
            Ads = 0;
            Notifications = 0;
        }
    }
}
