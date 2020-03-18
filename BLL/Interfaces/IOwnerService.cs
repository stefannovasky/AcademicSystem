using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IOwnerService : IService<Owner>
    {
        public Task<Response> AddCourse(Owner owner, Course course);
    }
}
