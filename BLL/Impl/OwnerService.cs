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
    public class OwnerService : IOwnerService
    {
        private IOwnerRepository _OwnerRepo;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public OwnerService(IOwnerRepository ownerRepository)
        {
            _OwnerRepo = ownerRepository;
        }
        public async Task<Response> Create(Owner item)
        {
            Response response = new Response();
            try
            {
                response = await _OwnerRepo.Create(item);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString()); 
                response.ErrorList.Add("Error on create Owner");
                response.Success = false;
                return response;
            }
        }

        public async Task<Response> Delete(int id)
        {
            try
            {
                Response response = await _OwnerRepo.Delete(id);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                Response r = new Response() { Success = false };
                r.ErrorList.Add("Error on delete Owner");
                return r;
            }
        }

        public async Task<DataResponse<Owner>> GetAll()
        {
            try
            {
                DataResponse<Owner> response = await _OwnerRepo.GetAll();
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                DataResponse<Owner> r = new DataResponse<Owner>() { Success = false };
                r.ErrorList.Add("Error on read Owners");
                return r;
            }
        }

        public async Task<DataResponse<Owner>> GetByID(int id)
        {
            try
            {
                DataResponse<Owner> response = await _OwnerRepo.GetByID(id);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                DataResponse<Owner> r = new DataResponse<Owner>() { Success = false };
                r.ErrorList.Add("Error on get Owner");
                return r;
            }
        }

        public async Task<DataResponse<Owner>> Update(Owner item)
        {
            DataResponse<Owner> response = new DataResponse<Owner>();
            try
            {
                response = await _OwnerRepo.Update(item);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.ErrorList.Add("Error on update Owner");
                response.Success = false;
                return response;
            }
        }

        public async Task<Response> AddCourse(Owner owner, Course course)
        {
            Response response = new Response();
            try
            {
                ValidationResult validationResponse = await new CourseValidator().ValidateAsync(course);
                if (!validationResponse.IsValid)
                {
                    response.Success = false;
                    response.ErrorList.Add("Validation Error");
                    return response;
                }
                response = await _OwnerRepo.AddCourse(owner, course);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Erro While adding course");
                return response;
            }
        }
    }
}
