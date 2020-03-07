using DAL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Impl
{
    public class UserRepository : IUserRepository
    {
        public Response Create(User item)
        {
            throw new NotImplementedException();
        }

        public Response Delete(int id)
        {
            throw new NotImplementedException();
        }

        public DataResponse<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public DataResponse<User> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public DataResponse<User> Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
