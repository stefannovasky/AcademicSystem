using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class StudentEvaluation
    {
        public Student Student { get; set; }
        public Evaluation Evaluation { get; set; }
        public int StudentID { get; set; }
        public int EvaluationID { get; set; }

    }
}
