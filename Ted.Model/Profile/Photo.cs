using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ted.Model
{
    public class Photo : Entity
    {
        [ForeignKey("User")]
        public Guid UserId { set; get; }
        public byte[] File { set; get; }
    }
}
