using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class StudentAttendance
    {
        public Student Student { get; set; }
        public Attendance Attendance { get; set; }
        public int StudentID { get; set; }
        public int AttendanceID { get; set; }
    }
}
