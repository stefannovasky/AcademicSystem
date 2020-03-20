using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicSystemApi.Models
{
    public class AttendanceViewModel
    {
        public DateTime Date { get; set; }
        public bool Value { get; set; }
        public int ClassID { get; set; }
        public int StudentID { get; set; }
    }
}
