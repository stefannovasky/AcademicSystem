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
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetOwner(int id)
        {
            try
            {
                DataResponse<Owner> response = await _service.GetByID(id);
                response.Data[0].User.Owner = null;

                foreach (var owner in response.Data)
                {
                    foreach (var course in owner.Courses)
                    {
                        course.Owner = null;
                    }
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
        }

        [HttpPost]
        [Authorize]
        public async Task<object> CreateOwner(Owner Owner)
        {
            try
            {
                Response response = await _service.Create(Owner);
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
        public async Task<object> UpdateOwner(Owner Owner)
        {
            try
            {
                Response response = await _service.Update(Owner);
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
        public async Task<object> DeleteOwner(int id)
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

        private int GetUserID()
        {
            string id = HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
            return Convert.ToInt32(id);
        }
    }
}