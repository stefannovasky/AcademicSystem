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
    [Route("subjects")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        ISubjectService _service;
        public SubjectController(ISubjectService service)
        {
            this._service = service;
        }

        [Authorize]
        public async Task<object> GetSubjects()
        {
            try
            {
                DataResponse<Subject> response = await _service.GetAll();

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
        public async Task<object> GetSubject(int id)
        {
            try
            {
                DataResponse<Subject> response = await _service.GetByID(id);

                response.Data.ForEach(subject => subject.Course.Subjects = null);

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
        public async Task<object> CreateSubject(Subject Subject)
        {
            try
            {
                Response response = await _service.Create(Subject);
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
        public async Task<object> UpdateSubject(Subject Subject, int id)
        {
            Subject.ID = id; 
            try
            {
                Response response = await _service.Update(Subject);
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
        public async Task<object> DeleteSubject(int id)
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