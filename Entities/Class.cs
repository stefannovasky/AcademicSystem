using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Class : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

        public int SubjectID { get; set; }
        public virtual Subject Subject { get; set; }
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }

        public virtual ICollection<StudentClass> Students { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<InstructorClass> Instructors { get; set; }
        public virtual ICollection<CoordinatorClass> Coordinators { get; set; }
    }
}
