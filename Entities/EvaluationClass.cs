using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class EvaluationClass
    {
        public int EvaluationID { get; set; }
        public Evaluation Evaluation { get; set; }
        public int ClassID { get; set; }
        public Class Class { get; set; }
    }
}
