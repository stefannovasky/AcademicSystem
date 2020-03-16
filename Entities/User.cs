using System;

namespace Entities
{
    public class User : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string State {get; set; }
        public string Name { get; set; }
        public virtual Student Student { get; set; }

        public virtual Owner Owner { get; set; }
        public virtual Coordinator Coordinator { get; set; }
        public virtual Instructor Instructor { get; set; }
    }


}
