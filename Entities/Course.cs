using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Course : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public string Name { get; set; }
        public string Period { get; set; }
        public ICollection<CourseClass> Classes { get; set; }
        public ICollection<SubjectCourse> Subjects { get; set; }
        public ICollection<OwnerCourse> Owners { get; set; }
    }
}
