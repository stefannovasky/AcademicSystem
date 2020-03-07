using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Attendance : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }

        public int ClassID { get; set; }
        public Class Class { get; set; }
        public int StudentID { get; set; }
        public Student Student { get; set; }
        //1 turma, 1 aluno
    }
}
