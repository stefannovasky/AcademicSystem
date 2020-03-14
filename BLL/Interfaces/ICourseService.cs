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
        Task<Response> AddClass(Course course, Class Class);
        Task<Response> AddSubject(Course course, Subject subject);
        Task<Response> AddOwner(Course course, Owner owner);
        Task<DataResponse<int>> CreateAndReturnId(Course item);
    }
}
