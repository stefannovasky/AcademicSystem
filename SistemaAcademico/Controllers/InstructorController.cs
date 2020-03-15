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
    [Route("instructors")]
    [ApiController]
    public class InstructorController : ControllerBase
    {

        IInstructorService _service;
        public InstructorController(IInstructorService service)
        {
            this._service = service;
        }

        [Authorize]
        public async Task<object> GetInstructors()
        {
            try
            {
                DataResponse<Instructor> response = await _service.GetAll();
                foreach (Instructor Instructor in response.Data)
                {
                    if (Instructor.User != null)
                        Instructor.User.Instructor = null;
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
        public async Task<object> GetInstructor(int id)
        {
            try
            {
                DataResponse<Instructor> response = await _service.GetByID(id);
                response.Data[0].User.Instructor = null;
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
        public async Task<object> CreateInstructor(Instructor Instructor)
        {
            try
            {
                Response response = await _service.Create(Instructor);
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
        public async Task<object> UpdateInstructor(Instructor Instructor)
        {
            try
            {
                Response response = await _service.Update(Instructor);
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
        public async Task<object> DeleteInstructor(int id)
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