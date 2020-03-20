using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicSystemApi.Models
{
    public class EvaluationViewModel
    {
        public bool Concluded { get; set; }
        public double Value { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public int ClassID { get; set; }
        public int StudentID { get; set; }

    }
}
