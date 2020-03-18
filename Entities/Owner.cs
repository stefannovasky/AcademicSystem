using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Owner : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public int UserID { get; set; }
        public  User User { get; set; }
        public  ICollection<OwnerCourse> Courses {get; set;}
    }   
}
