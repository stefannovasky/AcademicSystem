using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class StudentEvaluation
    {
        public int StudentID { get; set; }
        public Student Student { get; set; }
        public int EvaluationID { get; set; }
        public Evaluation Evaluation { get; set; }
    }
}
