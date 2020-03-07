using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class AttendanceClass
    {
        public int AttendanceID { get; set; }
        public Attendance Attendance { get; set; }
        public int ClassID { get; set; }
        public Class Class { get; set; }
    }
}
