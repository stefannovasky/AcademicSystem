using DAL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Impl
{
    public class CourseRepository : ICourseRepository
    {
        public Task<Response> Create(Course item)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Course>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Course>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Course>> Update(Course item)
        {
            throw new NotImplementedException();
        }
    }
}
