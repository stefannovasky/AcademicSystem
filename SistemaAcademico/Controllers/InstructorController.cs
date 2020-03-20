using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AcademicSystemApi.Extensions;
using AcademicSystemApi.Models;
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
        IUserService userService;
        ICoordinatorService coordinatorService;
        ICourseService courseService;
        IOwnerService ownerService;
        IStudentService studentService;
        public InstructorController(IInstructorService service, IUserService userService, ICoordinatorService coordinatorService, ICourseService courseService, IOwnerService ownerService, IStudentService studentService)
        {
            this._service = service;
            this.userService = userService;
            this.coordinatorService = coordinatorService;
            this.courseService = courseService;
            this.ownerService = ownerService;
            this.studentService = studentService;
        }
        /// <summary>
        /// pegar um instructor.
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetInstructor(int id)
        {
            try
            {
                DataResponse<Instructor> response = await _service.GetByID(id);
                if (await CheckPermissionToGetInstructor(response.Data[0]))
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
        ///     Metodo cria um instrcutor.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<object> CreateInstructor(InstructorViewModel model)
        {
            Instructor instructor = new SimpleAutoMapper<Instructor>().Map(model);

            try
            {
                if (await CheckPermissionToCreateUpdateInstructor(instructor)) {
                    Response response = await _service.Create(instructor);
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
        /// Atualiza um instructor.
        /// </summary>
        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<object> UpdateInstructor(InstructorViewModel model, int id)
        {
            Instructor instructor = new SimpleAutoMapper<Instructor>().Map(model);

            instructor.ID = id;
            try
            {
                if (await CheckPermissionToCreateUpdateInstructor(instructor))
                {
                    Response response = await _service.Update(instructor);
                    return this.SendResponse(response);
                }
                return Forbid();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// Metodo deleta um instructor.
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<object> DeleteInstructor(int id)
        {
            try
            {
                if (await CheckPermissionToDeleteInstructor(id))
                {
                    Response response = await _service.Delete(id);
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
        /// Metodo checa permissão de criação e atualização de um instrcutor.
        /// </summary>
        private async Task<bool> CheckPermissionToCreateUpdateInstructor(Instructor instructor)
        {
            User user = (await userService.GetByID(this.GetUserID())).Data[0];
            if (user.Instructor != null && user.Instructor.IsActive)
            {
                if (this.GetUserID() == instructor.UserID)
                {
                    return true;
                } 
            }
            return false;
        }
        /// <summary>
        ///     Verifica se o usuario autenticado tem autorização para deletar um instructor.
        /// </summary>
        private async Task<bool> CheckPermissionToDeleteInstructor(int id)
        {
            User user = (await userService.GetByID(this.GetUserID())).Data[0];
            Instructor instructor = (await _service.GetByID(id)).Data[0];
            if (user.Instructor != null && user.Instructor.IsActive && instructor != null && this.GetUserID() == instructor.UserID)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Verifica se o usuario autenticado tem autorização para
        /// </summary>
        private async Task<bool> CheckPermissionToGetInstructor(Instructor instructor) 
        {
            User user = (await userService.GetByID(this.GetUserID())).Data[0];
            if (user.Instructor.ID == instructor.ID)
            {
                return true;
            }
            foreach (CoordinatorClass coordinatorClass in (await coordinatorService.GetByID(user.Coordinator.ID)).Data[0].Classes)
            {
                if (instructor.Classes.Where(c => c.ClassID == coordinatorClass.ClassID).Any())
                {
                    return true;
                }
            }
            foreach (OwnerCourse ownerCourse in (await ownerService.GetByID(user.Owner.ID)).Data[0].Courses)
            {
                foreach (Class @class in (await courseService.GetByID(ownerCourse.CourseID)).Data[0].Classes)
                {
                    if (instructor.Classes.Where(c => c.ClassID == @class.ID).Any())
                    {
                        return true;
                    }
                }
            }
            foreach (InstructorClass instructorClass in (await _service.GetByID(user.Instructor.ID)).Data[0].Classes)
            {
                if (instructor.Classes.Where(c => c.ClassID == instructorClass.ClassID).Any()) 
                {
                    return true;
                }
            }
            foreach (StudentClass StudentClass in (await studentService.GetByID(user.Student.ID)).Data[0].Classes)
            {
                if (instructor.Classes.Where(c => c.ClassID == StudentClass.ClassID).Any())
                {
                    return true;
                }
            }
            return false;
        }
    }
}