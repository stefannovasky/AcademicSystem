using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<Response> AddOwner(Course course, Owner owner);
        Task<Response> AddSubject(Course course, Subject subject);
        Task<Response> AddClass(Course course, Class Class);
        Task<DataResponse<int>> CreateAndReturnID(Course item);
    }
}
