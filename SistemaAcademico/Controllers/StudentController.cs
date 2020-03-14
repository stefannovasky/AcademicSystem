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
    [Route("students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        IStudentService _service;
        public StudentController(IStudentService service)
        {
            this._service = service;
        }

        [Authorize]
        public async Task<DataResponse<Student>> GetStudents()
        {
            try
            {
                DataResponse<Student> response = await _service.GetAll();
                foreach (Student student in response.Data)
                {
                    if (student.User != null)
                        student.User.Student = null;
                }
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<DataResponse<Student>> GetStudent(int id)
        {
            try
            {
                DataResponse<Student> response = await _service.GetByID(id);
                response.Data[0].User.Student = null;
                return response;
            } catch (Exception e)
            {
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<object> CreateStudent(Student student)
        {
            try
            {
                Response response = await _service.Create(student);
                return new
                {
                    sucess = response.Success
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<Response> UpdateStudent(Student student)
        {
            try
            {
                Response response = await _service.Update(student);
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<Response> DeleteStudent(int id)
        {
            try
            {
                Response response = await _service.Delete(id);
                return response;
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