using BLL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class StudentService : IStudentService
    {
        public async Task<Response> Create(Student item)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Student>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Student>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Student>> Update(Student item)
        {
            throw new NotImplementedException();
        }
    }
}
