using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class CoordinatorClass
    {
        public int CoordinatorID { get; set; }
        public virtual Coordinator Coordinator { get; set; }
        public int ClassID { get; set; }
        public virtual Class Class { get; set; }
    }
}
