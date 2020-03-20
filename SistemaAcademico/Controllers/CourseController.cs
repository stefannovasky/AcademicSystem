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
    [Route("courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {        
        ICourseService _courseService;
        IUserService _userService;
        IOwnerService _ownerService;
        ICoordinatorService _coordinatorService;
        IClassService _classService;
        IInstructorService _instructorService;
        IStudentService _studentService;
        ISubjectService _subjectService;
        public CourseController(ICourseService service, IUserService userService, IOwnerService ownerService, ICoordinatorService coordinatorService, IClassService classService, IInstructorService instructorService, IStudentService studentService, ISubjectService subjectService)
        {
            this._courseService = service;
            this._userService = userService;
            this._ownerService = ownerService;
            this._coordinatorService = coordinatorService;
            this._classService = classService;
            this._instructorService = instructorService;
            this._studentService = studentService;
            this._subjectService = subjectService;
        }

        /// <summary>
        ///     Pega um Course através de seu ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetCourse(int id)
        {
            try
            {
                DataResponse<Course> response = await _courseService.GetByID(id);
                if (await this.CheckPermissionToReadCourse(response.Data[0]))
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
        ///     Cria um Course 
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<object> CreateCourse(CourseViewModel model)
        {
            Course course = new SimpleAutoMapper<Course>().Map(model);

            try
            {
                if (await this.CheckPermissionToCreateCourse(course))
                {
                    User user = (await _userService.GetByID(this.GetUserID())).Data[0];

                    if (course.Owners != null)
                    {
                        course.Owners.Clear();
                    }
                    
                    int id = (await _courseService.CreateAndReturnId(course)).Data[0];
                    course = (await _courseService.GetByID(id)).Data[0];
                    return this.SendResponse(await _courseService.AddOwner(course, user.Owner));
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
        ///     Altera um Course pelo corpo da requisição através de seu ID
        /// </summary>
        /// <param name="course"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<object> UpdateCourse(CourseViewModel model, int id)
        {
            Course course = new SimpleAutoMapper<Course>().Map(model);

            course.ID = id;
            try
            {
                User user = (await _userService.GetByID(this.GetUserID())).Data[0];
                if (await this.CheckPermissionToUpdateCourse(course))
                {
                    return this.SendResponse(await _courseService.Update(course));
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
        ///     Deleta um Course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<object> DeleteCourse(int id)
        {
            try
            {
                //this.SendResponse(await _courseService.Delete(id));
                if (await this.CheckPermissionToDeleteCourse(id))
                {
                    Response response = await _courseService.Delete(id);
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
        ///     Adiciona um Subject em determinado Course
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("subject")]
        [Authorize]
        public async Task<object> AddSubject(SubjectCourseViewModel model)
        {
            try
            {
                if (await this.CheckPermissionToAddSubject(model.CourseID))
                {
                    return await _courseService.AddSubject((await _courseService.GetByID(model.CourseID)).Data[0], (await _subjectService.GetByID(model.SubjectID)).Data[0]);
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
        ///     Adicionar um Owner em determinado Course 
        /// </summary>
        /// <param name="ownerCourse"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("owner")]
        [Authorize]
        public async Task<object> AddOwner(OwnerCourseViewModel model)
        {
            OwnerCourse ownerCourse = new SimpleAutoMapper<OwnerCourse>().Map(model);

            try
            {
                if (await this.CheckPermissionToAddOwner(ownerCourse))
                {
                    return await _courseService.AddOwner((await _courseService.GetByID(ownerCourse.CourseID)).Data[0], (await _ownerService.GetByID(ownerCourse.OwnerID)).Data[0]);
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
        ///     Verifica se o usuario logado tem permissão para criar um Course 
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToCreateCourse(Course course)
        {
            try
            {
                User user = (await _userService.GetByID(this.GetUserID())).Data[0];
                if (user.Owner != null && user.Owner.IsActive)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        ///     Verifica se o usuario logado tem permissao para deletar determinado Course 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToDeleteCourse(int id)
        {
            try
            {
                int ownerID = await this.VerifyIfUserIsOwnerAndReturnOwnerId(_userService);
                Course course = (await _courseService.GetByID(id)).Data[0];
                Owner owner = (await _ownerService.GetByID(ownerID)).Data[0];
                foreach (OwnerCourse ownerCourse in course.Owners)
                {
                    if (owner.Courses.Contains(ownerCourse))
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
        ///     Verifica se o usuario logado tem permissao para alterar determinado Course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToUpdateCourse(Course course)
        {
            try
            {
                User user = (await _userService.GetByID(this.GetUserID())).Data[0];
                if (user.Owner != null && user.Owner.IsActive)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];
                    foreach (OwnerCourse ownerCourse in course.Owners)
                    {
                        if (owner.Courses.Contains(ownerCourse))
                        {
                            return true;
                        }
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
        ///     Verifica se o usuario logado tem permissao para adicionar Subject em um determinado Course 
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToAddSubject(int courseID)
        {
            try
            {
                User user = (await _userService.GetByID(this.GetUserID())).Data[0];
                Owner userOwner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];
                if (userOwner.Courses.Where(c => c.CourseID == courseID).Any())
                {
                    return true;
                }

                return false; 
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        ///     Verifica se o usuario logado tem permissao para adicionar um Owner em um determinado Course 
        /// </summary>
        /// <param name="ownerCourse"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToAddOwner(OwnerCourse ownerCourse)
        {
            try
            {
                User user = (await _userService.GetByID(this.GetUserID())).Data[0];
                Owner userOwner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];
                if (userOwner.Courses.Where(c => c.CourseID == ownerCourse.CourseID).Any())
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false; 
            }
        }
        /// <summary>
        ///     Verifica se o usuario logado tem permissao para ler determinado Course 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToReadCourse(Course c)
        {
            User u = (await _userService.GetByID(this.GetUserID())).Data[0];

            // verify 
            bool hasPermissionToRead = false;
            if (u.Owner != null && u.Owner.IsActive)
            {
                Owner o = (await _ownerService.GetByID(u.Owner.ID)).Data[0];
                foreach (OwnerCourse ownerCourse in o.Courses)
                {
                    if (c.Owners.Where(owner => owner.CourseID == ownerCourse.OwnerID).Any())
                    {
                        hasPermissionToRead = true;
                    }
                }
            }
            if (u.Student != null && u.Student.IsActive)
            {
                Student student = (await this._studentService.GetByID(u.Student.ID)).Data[0];
                foreach (StudentClass studentClass in student.Classes)
                {
                    Class Class = (await this._classService.GetByID(studentClass.ClassID)).Data[0];
                    if (Class.CourseID == c.ID)
                    {
                        hasPermissionToRead = true;
                    }
                }
            }
            if (u.Coordinator != null && u.Coordinator.IsActive)
            {
                Coordinator coordinator = (await this._coordinatorService.GetByID(u.Coordinator.ID)).Data[0];
                foreach (CoordinatorClass coordinatorClass in coordinator.Classes)
                {
                    Class Class = (await this._classService.GetByID(coordinatorClass.ClassID)).Data[0];
                    if (Class.Coordinators.Where(c => c.CoordinatorID == coordinator.ID).Any())
                    {
                        hasPermissionToRead = true;
                    }
                }
            }

            return hasPermissionToRead;
        }
    }
}