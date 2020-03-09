using BLL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class EvaluationService : IEvaluationService
    {
        public async Task<Response> Create(Evaluation item)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Evaluation>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Evaluation>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Evaluation>> Update(Evaluation item)
        {
            throw new NotImplementedException();
        }
    }
}
