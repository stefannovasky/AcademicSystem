using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicSystemApi.Models
{
    public class UpdateUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string State { get; set; }
        public string Name { get; set; }
    }
}
