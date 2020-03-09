using BLL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class InstructorService : IInstructorService
    {
        public async Task<Response> Create(Instructor item)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Instructor>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Instructor>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Instructor>> Update(Instructor item)
        {
            throw new NotImplementedException();
        }
    }
}
