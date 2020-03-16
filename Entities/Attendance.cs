using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Attendance : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public DateTime Date { get; set; }
        public bool Value { get; set; }

        public int ClassID { get; set; }
        public virtual Class Class { get; set; }
        public int StudentID { get; set; }
        public virtual Student Student { get; set; }
    }
}
