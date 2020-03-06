﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Class : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

        public int SubjectID { get; set; }
        public Subject Subject { get; set; }
        public int CourseID { get; set; }
        public Course Course { get; set; }

        public ICollection<Student> Students { get; set; }
        public ICollection<Evaluation> Evaluations { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<Instructor> Instructors { get; set; }
        public ICollection<Coordinator> Coordinators { get; set; }
    }
}
