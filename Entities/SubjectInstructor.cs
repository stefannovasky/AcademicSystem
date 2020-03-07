using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class SubjectInstructor
    {
        public int SubjectID { get; set; }
        public Subject Subject{ get; set; }
        public int InstructorID { get; set; }
        public Instructor Instructor { get; set; }
    }
}
