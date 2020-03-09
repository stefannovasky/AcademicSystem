using BLL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class CourseService : ICourseService
    {
        public async Task<Response> Create(Course item)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Course>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Course>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Course>> Update(Course item)
        {
            throw new NotImplementedException();
        }
    }
}
