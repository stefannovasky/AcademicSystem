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
    [Route("evaluations")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {

        IEvaluationService _service;
        IUserService _userService;
        IStudentService _studentService;
        IInstructorService _instructorService;
        public EvaluationController(IEvaluationService service, IUserService userService, IStudentService studentService, IInstructorService instructorService)
        {
            this._service = service;
            this._userService = userService;
            this._studentService = studentService;
            this._instructorService = instructorService;
        }

        /*
        [Authorize]
        public async Task<object> GetEvaluations()
        {
            try
            {
                DataResponse<Evaluation> response = await _service.GetAll();

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
        */

        private async Task<bool> PermissionCheckToReadEvaluation(Evaluation e)
        {
            bool hasPermissionToRead = false;

            User user = (await this._userService.GetByID(this.GetUserID())).Data[0];
            if (user.Student != null)
            {
                Student student = (await this._studentService.GetByID(user.Student.ID)).Data[0];
                // ver se a evaluation StudentID == Student.ID
                if (student.Evaluations.Where(evaluation => evaluation.StudentID == e.StudentID).ToList().Count > 0)
                {
                    hasPermissionToRead = true;
                }
            }
            if (user.Instructor != null)
            {
                Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];
                if (instructor.Classes.Where(instructorClass => instructorClass.ClassID == e.ClassID).ToList().Count > 0)
                {
                    hasPermissionToRead = true;
                }
            }

            return hasPermissionToRead;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetEvaluation(int id)
        {
            try
            {
                DataResponse<Evaluation> response = await _service.GetByID(id);

  
                if (response.HasError())
                {
                    return response;
                }

                if (await this.PermissionCheckToReadEvaluation(response.Data[0]))
                {
                    return this.SendResponse(response);
                }

                return Forbid();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<object> CreateEvaluation(Evaluation Evaluation)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];
                if (user.Instructor == null)
                {
                    return Forbid();
                }
                Response response = await _service.Create(Evaluation);
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
        public async Task<object> UpdateEvaluation(Evaluation Evaluation, int id)
        {
            Evaluation.ID = id;
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];
                if (user.Instructor == null)
                {
                    return Forbid();
                }
                Response response = await _service.Update(Evaluation);
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
        public async Task<object> DeleteEvaluation(int id)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];
                if (user.Instructor == null)
                {
                    return Forbid();
                }
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