using BLL.Interfaces;
using BLL.Validators;
using DAL.Impl;
using DAL.Interfaces;
using Entities;
using FluentValidation.Results;
using log4net;
using Shared;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class EvaluationService : IEvaluationService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IEvaluationRepository _repository;
        public EvaluationService(IEvaluationRepository repository)
        {
            this._repository = repository;
        }

        public async Task<Response> Create(Evaluation item)
        {
            Response response = new Response();
            try
            {
                ValidationResult validationResponse = await new EvaluationValidator().ValidateAsync(item);
                if (!validationResponse.IsValid)
                {
                    response.Success = false;
                    response.ErrorList.Add("Validation Error");
                    return response;
                }
                response = await _repository.Create(item);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.ErrorList.Add("Error while creating Evaluation.");
                response.Success = false;
                return response;
            }
        }

        public async Task<Response> Delete(int id)
        {
            Response response = new Response();
            try
            {
                response = await _repository.Delete(id);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Erro while deleting Evaluation");
                return response;
            }
        }

        public async Task<DataResponse<Evaluation>> GetAll()
        {
            DataResponse<Evaluation> response = new DataResponse<Evaluation>();
            try
            {
                response = await _repository.GetAll();
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Erro inesperado");
                return response;
            }
        }

        public async Task<DataResponse<Evaluation>> GetByID(int id)
        {
            DataResponse<Evaluation> response = new DataResponse<Evaluation>();
            try
            {
                response = await _repository.GetByID(id);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Erro inesperado");
                return response;
            }
        }

        public async Task<DataResponse<Evaluation>> Update(Evaluation item)
        {
            DataResponse<Evaluation> response = new DataResponse<Evaluation>();
            try
            {
                ValidationResult validationResponse = await new EvaluationValidator().ValidateAsync(item);
                if (!validationResponse.IsValid)
                {
                    response.Success = false;
                    response.ErrorList.Add("Validation Error");
                    return response;
                }
                response = await _repository.Update(item);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Erro inesperado");
                return response;
            }
        }
    }
}
