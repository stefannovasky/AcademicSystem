using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IOwnerRepository : IRepository<Owner>
    {
        Task<Response> AddCourse(Owner owner, Course course);
    }
}
