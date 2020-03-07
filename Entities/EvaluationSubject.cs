using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class EvaluationSubject
    {
        public int EvaluationID { get; set; }
        public Evaluation Evaluation { get; set; }
        public int SubjectID { get; set; }
        public Subject Subject { get; set; }
    }
}
