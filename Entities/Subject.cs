using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Subject : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SubjectInstructor> Instructors { get; set; }
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
    }
}
