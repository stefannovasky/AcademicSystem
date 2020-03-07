using DAL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Impl
{
    public class SubjectRepository : ISubjectRepository
    {
        public Task<Response> Create(Subject item)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Subject>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Subject>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Subject>> Update(Subject item)
        {
            throw new NotImplementedException();
        }
    }
}
