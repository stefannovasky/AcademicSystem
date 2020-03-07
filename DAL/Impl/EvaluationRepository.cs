using DAL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Impl
{
    public class EvaluationRepository : IEvaluationRepository
    {
        public Task<Response> Create(Evaluation item)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Evaluation>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Evaluation>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Evaluation>> Update(Evaluation item)
        {
            throw new NotImplementedException();
        }
    }
}
