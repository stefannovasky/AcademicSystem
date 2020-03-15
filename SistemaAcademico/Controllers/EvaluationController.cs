using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace AcademicSystemApi.Controllers
{
    [Route("evaluations")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {

        IEvaluationService _service;
        public EvaluationController(IEvaluationService service)
        {
            this._service = service;
        }

        [Authorize]
        public async Task<object> GetEvaluations()
        {
            try
            {
                DataResponse<Evaluation> response = await _service.GetAll();

                return new
                {
                    success = response.Success,
                    data = response.Success ? response.Data : null
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetEvaluation(int id)
        {
            try
            {
                DataResponse<Evaluation> response = await _service.GetByID(id);

                //response.Data.ForEach(Evaluation => Evaluation.Course.Evaluations = null);

                return new
                {
                    success = response.Success,
                    data = response.Success ? response.Data : null
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<object> CreateEvaluation(Evaluation Evaluation)
        {
            try
            {
                Response response = await _service.Create(Evaluation);
                return new
                {
                    success = response.Success
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }


        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<object> UpdateEvaluation(Evaluation Evaluation, int id)
        {
            Evaluation.ID = id;
            try
            {
                Response response = await _service.Update(Evaluation);
                return new
                {
                    success = response.Success
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<object> DeleteEvaluation(int id)
        {
            try
            {
                Response response = await _service.Delete(id);

                return new
                {
                    success = response.Success
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}