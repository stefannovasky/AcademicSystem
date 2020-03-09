using BLL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class OwnerService : IOwnerService
    {
        public async Task<Response> Create(Owner item)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Owner>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Owner>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Owner>> Update(Owner item)
        {
            throw new NotImplementedException();
        }
    }
}
