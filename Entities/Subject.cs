using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Subject : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public string Name { get; set; }

        public ICollection<SubjectInstructor> Instructors { get; set; }
        public int CourseID { get; set; }
        public ICollection<Class> Classes { get; set; }
    }
}
