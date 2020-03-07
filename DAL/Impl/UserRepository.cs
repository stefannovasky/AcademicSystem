using DAL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Impl
{
    public class UserRepository : IUserRepository
    {
        public Task<Response> Create(User item)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<User>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<User>> Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
