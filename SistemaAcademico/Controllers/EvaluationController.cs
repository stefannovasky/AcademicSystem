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
using log4net.Config;
using log4net;
using System.Reflection;

namespace AcademicSystemApi.Controllers
{
    [Route("evaluations")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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


        private async Task<bool> PermissionCheckToReadEvaluation(Evaluation e)
        {
            bool hasPermissionToRead = false;

            User user = (await this._userService.GetByID(this.GetUserID())).Data[0];
            if (user.Student != null && user.Student.IsActive)
            {
                Student student = (await this._studentService.GetByID(user.Student.ID)).Data[0];
                if (student.Evaluations.Where(evaluation => evaluation.StudentID == e.StudentID).Any())
                {
                    hasPermissionToRead = true;
                }
            }
            if (user.Instructor != null && user.Instructor.IsActive)
            {
                Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];
                if (instructor.Classes.Where(instructorClass => instructorClass.ClassID == e.ClassID).Any())
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
                Response.StatusCode = StatusCode(500).StatusCode;
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

                if (user.Instructor != null && user.Instructor.IsActive)
                {
                    Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];

                    if (instructor.Classes.Where(c => c.ClassID == Evaluation.ClassID).Any())
                    {
                        Response response = await _service.Create(Evaluation);
                        return this.SendResponse(response);
                    }
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
        [Route("{id}")]
        public async Task<object> UpdateEvaluation(Evaluation Evaluation, int id)
        {
            Evaluation.ID = id;
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Instructor != null && user.Instructor.IsActive)
                {
                    Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];

                    if (instructor.Classes.Where(c => c.ClassID == Evaluation.ClassID).Any())
                    {
                        Response response = await _service.Update(Evaluation);
                        return this.SendResponse(response);
                    }
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
        public async Task<object> DeleteEvaluation(int id)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Instructor != null && user.Instructor.IsActive)
                {
                    Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];
                    Evaluation Evaluation = (await this._service.GetByID(id)).Data[0];
                    if (instructor.Classes.Where(c => c.ClassID == Evaluation.ClassID).Any())
                    {
                        Response response = await _service.Delete(id);
                        return this.SendResponse(response);
                    }
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