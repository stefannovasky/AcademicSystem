using BLL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class ClassService : IClassService
    {
        public Task<Response> Create(Class item)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Class>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Class>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Class>> Update(Class item)
        {
            throw new NotImplementedException();
        }
    }
}
