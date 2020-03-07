using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class CoordinatorClass
    {
        public int CoordinatorID { get; set; }
        public Coordinator Coordinator { get; set; }
        public int ClassID { get; set; }
        public Class Class { get; set; }
    }
}
