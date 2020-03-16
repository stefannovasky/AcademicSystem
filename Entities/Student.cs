using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Student : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Evaluation> Evaluations {get; set; }
        public virtual ICollection<StudentClass> Classes { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
