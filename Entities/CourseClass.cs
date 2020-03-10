using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class CourseClass
    {
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public int ClassID { get; set; }
        public Class Class { get; set; }
    }
}
