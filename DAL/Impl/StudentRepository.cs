using DAL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Impl
{
    public class StudentRepository : IStudentRepository
    {
        public Response Create(Student item)
        {
            throw new NotImplementedException();
        }

        public Response Delete(int id)
        {
            throw new NotImplementedException();
        }

        public DataResponse<Student> GetAll()
        {
            throw new NotImplementedException();
        }

        public DataResponse<Student> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public DataResponse<Student> Update(Student item)
        {
            throw new NotImplementedException();
        }
    }
}
