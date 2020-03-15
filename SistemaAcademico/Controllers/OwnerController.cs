using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AcademicSystemApi.Extensions;
using BLL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace AcademicSystemApi.Controllers
{
    [Route("owners")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        IOwnerService _service;
        public OwnerController(IOwnerService service)
        {
            this._service = service;
        }

        [Authorize]
        public async Task<object> GetOwners()
        {//não sera usado
            /*
            try
            {
                DataResponse<Owner> response = await _service.GetAll();
                foreach (Owner Owner in response.Data)
                {
                    if (Owner.User != null)
                        Owner.User.Owner = null;
                }
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
            */
            DataResponse<Student> response = new DataResponse<Student>();
            response.Success = false;
            response.ErrorList.Add("Permission Denied");
            return response;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetOwner(int id)
        {
            try
            {
                bool isPermited = false;
                DataResponse<Owner> response = await _service.GetByID(id);
                response.Data[0].User.Owner = null;

                foreach (var owner in response.Data)
                {
                    foreach (var course in owner.Courses)
                    {
                        course.Owner = null;
                    }
                }
                if (this.GetUserID() == response.Data[0].UserID)
                {
                    return response;
                }
                DataResponse<Student> responseError = new DataResponse<Student>();
                responseError.Success = false;
                responseError.ErrorList.Add("Permission Denied");
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<object> CreateOwner(Owner Owner)
        {
            try
            {
                if(this.GetUserID() == Owner.UserID)
                {
                    Response response = await _service.Create(Owner);
                    return response;
                }
                DataResponse<Student> responseError = new DataResponse<Student>();
                responseError.Success = false;
                responseError.ErrorList.Add("Permission Denied");
                return responseError;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<object> UpdateOwner(Owner Owner)
        {
            try
            {
                if (this.GetUserID() == Owner.UserID)
                {
                    Response response = await _service.Update(Owner);
                    return response;
                }
                DataResponse<Student> responseError = new DataResponse<Student>();
                responseError.Success = false;
                responseError.ErrorList.Add("Permission Denied");
                return responseError;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<object> DeleteOwner(int id)
        {
            try
            {
                if (this.GetUserID() == (await _service.GetByID(id)).Data[0].UserID)
                {
                    Response response = await _service.Delete(id);
                    return response;
                }
                DataResponse<Student> responseError = new DataResponse<Student>();
                responseError.Success = false;
                responseError.ErrorList.Add("Permission Denied");
                return responseError;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}