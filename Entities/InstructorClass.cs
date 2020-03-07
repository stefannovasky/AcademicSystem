using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class InstructorClass
    {
        public int InstructorID { get; set; }
        public Instructor Instructor { get; set; }
        public int ClassID { get; set; }
        public Class Category { get; set; }
    }
}
