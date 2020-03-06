using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    internal interface IEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
    }
}
