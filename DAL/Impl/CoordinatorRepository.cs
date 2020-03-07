using DAL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Impl
{
    public class CoordinatorRepository : ICoordinatorRepository
    {
        public Task<Response> Create(Coordinator item)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Coordinator>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Coordinator>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Coordinator>> Update(Coordinator item)
        {
            throw new NotImplementedException();
        }
    }
}
