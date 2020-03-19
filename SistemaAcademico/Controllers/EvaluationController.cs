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
        /// <summary>
        ///     Pega uma Evaluation através de seu ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Cria uma Evaluation 
        /// </summary>
        /// <param name="evaluation"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<object> CreateEvaluation(Evaluation evaluation)
        {
            try
            {
                if (await this.CheckPermissionToCreateEvaluation(evaluation))
                {
                    Response response = await _service.Create(evaluation);
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

        /// <summary>
        ///     Altera uma Evaluation pelo corpo da requisição através de seu ID 
        /// </summary>
        /// <param name="Evaluation"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<object> UpdateEvaluation(Evaluation Evaluation, int id)
        {
            Evaluation.ID = id;
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (await this.CheckPermissionToUpdateEvaluation(Evaluation))
                {
                    Response response = await _service.Update(Evaluation);
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

        /// <summary>
        ///     Deleta uma Evaluation 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<object> DeleteEvaluation(int id)
        {
            try
            {
                if (await this.CheckPermissionToDeleteEvaluation(id))
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


        /// <summary>
        ///     Verifica se o usuário logado tem permissão para criar uma Evaluation
        /// </summary>
        /// <param name="evaluation"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToCreateEvaluation(Evaluation evaluation)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Instructor != null && user.Instructor.IsActive)
                {
                    Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];

                    if (instructor.Classes.Where(c => c.ClassID == evaluation.ClassID).Any())
                    {
                        return true; 
                    }
                }

                return false; 
            }
            catch (Exception)
            {
                return false; 
            }
        }
        /// <summary>
        ///     Verifica se o usuário logado tem permissão para alterar determinada Evaluation
        /// </summary>
        /// <param name="evaluation"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToUpdateEvaluation(Evaluation evaluation)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Instructor != null && user.Instructor.IsActive)
                {
                    Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];

                    if (instructor.Classes.Where(c => c.ClassID == evaluation.ClassID).Any())
                    {
                        return true; 
                    }
                }

                return false; 
            }
            catch (Exception)
            {
                return false; 
            }
        }
        /// <summary>
        ///     Verifica se o usuario logado tem permissão para deletar determinada Evaluation 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToDeleteEvaluation(int id)
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
                        return true; 
                    }
                }
                return false; 
            }
            catch (Exception)
            {
                return false; 
            }
        }
        /// <summary>
        ///     Verifica se o usuário logado tem permissao para deletar determinada Evaluation
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
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
    }
}