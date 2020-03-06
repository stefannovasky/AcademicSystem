using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Course : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public string Name { get; set; }
        public string Period { get; set; }

        //n notas, 1 diciplina, n alunos, n frequencias
    }
}
