using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class OwnerCourse
    {
        public int OwnerID { get; set; }
        public virtual Owner Owner { get; set; }
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }
    }
}
