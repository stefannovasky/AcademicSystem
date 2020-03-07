using DAL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Impl
{
    public class InstructorRepository : IInstructorRepository
    {
        public Task<Response> Create(Instructor item)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Instructor>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Instructor>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Instructor>> Update(Instructor item)
        {
            throw new NotImplementedException();
        }
    }
}
