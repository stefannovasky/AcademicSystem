using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Instructor : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public ICollection<SubjectInstructor> Subjects { get; set; }
        public ICollection<InstructorClass> Classes { get; set; }
    }
}
