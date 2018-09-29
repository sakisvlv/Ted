using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ted.Model.Ads
{
    public class GlobalString : Entity
    {
        public string Word { get; set; }
    }
}
