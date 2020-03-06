using System;

namespace Entities
{
    public class User : IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
