using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Evaluation : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public bool Concluded { get; set; }
        public double Value { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public int ClassID { get; set; }
        public Class Class { get; set; }
        public int StudentID { get; set; }
        public Student Student { get; set; }
    }
}
