using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class SubjectClass
    {
        public int SubjectID { get; set; }
        public Subject Subject { get; set; }
        public Class Class { get; set; }
        public int ClassID { get; set; }
    }
}
