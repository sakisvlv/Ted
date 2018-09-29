﻿using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Ads;

namespace Ted.Model.Auth
{
    public class SkillKnn : Entity
    {
        public User User { get; set; }
        public GlobalString GlobalString { get; set; }
        public int Count { get; set; }
        public int Weight { get; set; }
    }
}
