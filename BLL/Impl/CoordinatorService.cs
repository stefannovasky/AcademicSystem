using BLL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    class CoordinatorService : ICoordinatorService
    {
        public async Task<Response> Create(Coordinator item)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Coordinator>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Coordinator>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Coordinator>> Update(Coordinator item)
        {
            throw new NotImplementedException();
        }
    }
}
