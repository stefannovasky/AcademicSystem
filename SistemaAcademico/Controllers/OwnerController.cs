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
        {
            try
            {
                return Forbid();
            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCode(500).StatusCode;
                return null;
            }
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetOwner(int id)
        {
            try
            {
                DataResponse<Owner> response = await _service.GetByID(id);
                if (this.GetUserID() == response.Data[0].UserID)
                {
                    return this.SendResponse(response);
                }
                return Forbid();
            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCode(500).StatusCode;
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
                    return this.SendResponse(response);
                }
                return Forbid();
            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCode(500).StatusCode;
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
                    return this.SendResponse(response);
                }
                return Forbid();
            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCode(500).StatusCode;
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
                    return this.SendResponse(response);
                }
                return Forbid();
            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCode(500).StatusCode;
                return null;
            }
        }
    }
}