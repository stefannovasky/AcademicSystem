using DAL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Impl
{
    public class OwnerRepository : IOwnerRepository
    {
        public Task<Response> Create(Owner item)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Owner>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Owner>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Owner>> Update(Owner item)
        {
            throw new NotImplementedException();
        }
    }
}
