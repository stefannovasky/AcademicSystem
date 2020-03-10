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
    public class ClassService : IClassService
    {
        private IClassRepository _repository = new ClassRepository();

        public async Task<Response> Create(Class item)
        {
            Response response = new Response();
            try
            {
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
                response.ErrorList.Add("Erro while deleting Class");
                return response;
            }
        }

        public async Task<DataResponse<Class>> GetAll()
        {
            DataResponse<Class> response = new DataResponse<Class>();
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

        public async Task<DataResponse<Class>> GetByID(int id)
        {
            DataResponse<Class> response = new DataResponse<Class>();
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

        public async Task<DataResponse<Class>> Update(Class item)
        {
            DataResponse<Class> response = new DataResponse<Class>();
            try
            {
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
