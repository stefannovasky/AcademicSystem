using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class ClassOwner
    {
        public int ClassID { get; set; }
        public Class Class { get; set; }
        public int OwnerID { get; set; }
        public Owner Owner { get; set; }
    }
}
