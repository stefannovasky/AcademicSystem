using DAL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Impl
{
    public class StudentRepository : IStudentRepository
    {
        public Task<Response> Create(Student item)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Student>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Student>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Student>> Update(Student item)
        {
            throw new NotImplementedException();
        }
    }
}
