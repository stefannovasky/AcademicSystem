using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IInstructorService : IService<Instructor>
    {
        public Task<Response> AddSubject(Instructor instructor, Subject subject);
        public Task<Response> AddClass(Instructor instructor, Class Class)
    }
}
