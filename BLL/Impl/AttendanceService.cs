using BLL.Interfaces;
using BLL.Validators;
using DAL.Impl;
using DAL.Interfaces;
using Entities;
using FluentValidation.Results;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class AttendanceService : IAttendanceService
    {
        private IAttendanceRepository _repository;

        public AttendanceService(IAttendanceRepository repository)
        {
            this._repository = repository;
        }

        public async Task<Response> Create(Attendance item)
        {
            Response response = new Response();
            try
            {
                ValidationResult validationResponse = await new AttendanceValidator().ValidateAsync(item);
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
                response.ErrorList.Add("Error while creating Service.");
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
                response.Success = false;
                response.ErrorList.Add("Erro while deleting Attendance");
                return response;
            }
        }

        public async Task<DataResponse<Attendance>> GetAll()
        {
            DataResponse<Attendance> response = new DataResponse<Attendance>();
            try
            {
                response = await _repository.GetAll();
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Erro inesperado");
                return response;
            }
        }

        public async Task<DataResponse<Attendance>> GetByID(int id)
        {
            DataResponse<Attendance> response = new DataResponse<Attendance>();
            try
            {
                response = await _repository.GetByID(id);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Erro inesperado");
                return response;
            }
        }

        public async Task<DataResponse<Attendance>> Update(Attendance item)
        {
            DataResponse<Attendance> response = new DataResponse<Attendance>();
            try
            {
                ValidationResult validationResponse = await new AttendanceValidator().ValidateAsync(item);
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
                response.Success = false;
                response.ErrorList.Add("Erro inesperado");
                return response;
            }
        }
    }
}
