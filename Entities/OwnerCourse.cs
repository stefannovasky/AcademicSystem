using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class OwnerCourse
    {
        public int OwnerID { get; set; }
        public Owner Owner { get; set; }
        public int CourseID { get; set; }
        public Course Course { get; set; }
    }
}
