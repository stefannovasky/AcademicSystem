using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        Task<Response> AddSubject(Instructor instructor, Subject subject);
        Task<Response> AddClass(Instructor instructor, Class Class);
    }
}
