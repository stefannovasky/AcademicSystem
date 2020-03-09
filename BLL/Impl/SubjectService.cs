using BLL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class SubjectService : ISubjectService
    {
        public async Task<Response> Create(Subject item)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Subject>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Subject>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Subject>> Update(Subject item)
        {
            throw new NotImplementedException();
        }
    }
}
