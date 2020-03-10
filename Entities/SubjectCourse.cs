using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class SubjectCourse
    {
        public int CourseID { get; set; }
        public Class Class { get; set; }
        public Subject Subject { get; set; }
        public int SubjectID { get; set; }
    }
}
