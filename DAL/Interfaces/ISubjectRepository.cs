using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<Response> AddInstructor(Subject subject, Instructor instructor);
        Task<Response> AddClass(Subject subject, Class Class);
    }
}
