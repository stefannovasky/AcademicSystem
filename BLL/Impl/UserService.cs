using BLL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class UserService : IUserService
    {
        public async Task<Response> Create(User item)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<User>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<User>> Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
