using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICourseService : IService<Course>
    {
        public Task<Response> AddClass(Course course, Class Class);
        public Task<Response> AddSubject(Course course, Subject subject);
        public Task<Response> AddOwner(Course course, Owner owner);
    }
}
