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
using Newtonsoft.Json;
using Shared;

namespace AcademicSystemApi.Controllers
{
    [Route("classes")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        IClassService _classService;
        ICourseService _courseService;
        IUserService _userService;
        IOwnerService _ownerService;
        ICoordinatorService _coordinatorService;
        IInstructorService _instructorService;
        public ClassController(IClassService classService, ICourseService courseService, IUserService userService, IOwnerService ownerService, ICoordinatorService coordinatorService, IInstructorService instructorService)
        {
            this._classService = classService;
            this._courseService = courseService;
            this._userService = userService;
            this._ownerService = ownerService;
            this._coordinatorService = coordinatorService;
            this._instructorService = instructorService;
        }

        /// <summary>
        ///     Cria uma Class
        /// </summary>
        /// <param name="Class"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<object> Create(Class Class)
        {
            try
            {
                if (await this.CheckPermissionToCreateOrUpdateClass(Class))
                {
                    Response response = await _classService.Create(Class);
                    return response;
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
        ///     Pega um Class através de seu ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetClass(int id)
        {
            try
            {
                DataResponse<Class> response = await _classService.GetByID(id);

                // verify 
                Class Class = (await _classService.GetByID(id)).Data[0];
                if (await this.CheckPermissionToReadClass(Class))
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
        ///     Deleta uma Class através de seu ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<object> DeleteClass(int id)
        {
            try
            {
                if (await this.CheckPermissionToDeleteClass(id))
                {
                    Response response = await _classService.Delete(id);
                    return response; 
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
        ///     Adiciona um Student em uma Class
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("student")]
        public async Task<object> AddStudent([FromBody] StudentClass item)
        {
            try
            {
                if (await this.CheckPermissionToAddStudent(item))
                {
                    Response response = await _classService.AddStudent(new Class() { ID = item.ClassID }, new Student() { ID = item.StudentID });
                    return new
                    {
                        success = response.Success
                    };
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
        ///     Adiciona um Instrucor em uma Class
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("instructor")]
        public async Task<object> AddInstructor([FromBody] InstructorClass item)
        {
            try
            {
                if (await this.CheckPermissionToAddInstructor(item))
                {
                    Response response = await _classService.AddInstructor(new Class() { ID = item.ClassID }, new Instructor() { ID = item.InstructorID });
                    return new
                    {
                        success = response.Success
                    };
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
        ///     Adiciona um Coordinator em uma Class
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("coordinator")]
        public async Task<object> AddCoordinator([FromBody] CoordinatorClass item)
        {
            try
            {
                if (await CheckPermissionToAddCoordinator(item))
                {
                    Response response = await _classService.AddCoordinator(new Class() { ID = item.ClassID }, new Coordinator() { ID = item.CoordinatorID });
                    return response; 
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
        ///     Altera uma Class pelo corpo da requisição através de seu ID
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("{id}")]
        public async Task<object> Update(Class Class, int id)
        {
            Class.ID = id; 
            try
            {
                if (await this.CheckPermissionToCreateOrUpdateClass(Class))
                {
                    Response response = await _classService.Update(Class);
                    return response;
                }

                return Forbid();
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCode(500).StatusCode;
                return null;
            }
        }
        
        /// <summary>
        ///     Verifica se o usuário logado tem permissão para criar ou alterar determinada Class
        /// </summary>
        /// <param name="Class"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToCreateOrUpdateClass(Class Class)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Owner != null && user.Owner.IsActive)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];

                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == Class.CourseID).Any())
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
        ///     Verifica se o usuário logado tem permissão para deletar determinada Class
        /// </summary>
        /// <param name="Class"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToDeleteClass(int id)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Owner != null && user.Owner.IsActive)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];
                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == id).Any())
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
        ///     Verifica se o usuário logado tem permissão adicionar um Student em determinada Class
        /// </summary>
        /// <param name="Class"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToAddStudent(StudentClass item)
        {
            try
            {
                bool isPermited = false;
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Owner != null && user.Owner.IsActive)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];
                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == item.ClassID).Any())
                        {
                            isPermited = true;
                        }
                    }
                }
                if (!isPermited)
                {
                    if (user.Instructor != null && user.Instructor.IsActive)
                    {
                        Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];
                        if (instructor.Classes.Where(ic => ic.ClassID == item.ClassID).Any())
                        {
                            isPermited = true;
                        }
                    }
                }
                if (!isPermited)
                {
                    if (user.Coordinator != null && user.Coordinator.IsActive)
                    {
                        Coordinator coordinator = (await this._coordinatorService.GetByID(user.Instructor.ID)).Data[0];
                        if (coordinator.Classes.Where(cc => cc.ClassID == item.ClassID).Any())
                        {
                            isPermited = true;
                        }
                    }
                }

                return isPermited;
            }
            catch (Exception)
            {
                return false; 
            }
        }
        /// <summary>
        ///     Verifica se o usuário logado tem permissão adicionar um Instructor em determinada Class
        /// </summary>
        /// <param name="Class"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToAddInstructor(InstructorClass item)
        {
            try
            {
                bool isPermited = false;
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Owner == null && user.Coordinator == null)
                {
                    return false; 
                }

                Coordinator coordinator = (await _coordinatorService.GetByID(user.Coordinator.ID)).Data[0];
                if (coordinator.Classes.Where(c => c.ClassID == item.ClassID).Any())
                {
                    isPermited = true;
                }

                if (!isPermited)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];
                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == item.ClassID).Any())
                        {
                            isPermited = true;
                        }
                    }
                }

                return isPermited;
            }
            catch (Exception)
            {
                return false; 
            }
        }
        /// <summary>
        ///     Verifica se o usuário logado tem permissão adicionar um Coordinator em determinada Class
        /// </summary>
        /// <param name="Class"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToAddCoordinator(CoordinatorClass item)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Owner != null && user.Owner.IsActive)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];
                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == item.ClassID).Any())
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
        ///     Verifica se o usuário logado tem permissão para ler determinada Class
        /// </summary>
        /// <param name="Class"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToReadClass(Class Class)
        {
            bool hasPermissionToRead = false;
            int userID = this.GetUserID();
            User u = (await this._userService.GetByID(userID)).Data[0];

            if (u.Student != null && u.Student.IsActive)
            {
                if (Class.Students != null)
                {
                    foreach (StudentClass student in Class.Students)
                    {
                        if (student.StudentID == u.Student.ID)
                        {
                            hasPermissionToRead = true;
                        }
                    }
                }
            }
            if (!hasPermissionToRead)
            {
                if (Class.Instructors != null && u.Instructor.IsActive)
                {
                    foreach (InstructorClass instructor in Class.Instructors)
                    {
                        if (instructor.InstructorID == u.Instructor.ID)
                        {
                            hasPermissionToRead = true;
                        }
                    }
                }
            }
            if (!hasPermissionToRead)
            {
                Coordinator coordinator = (await this._coordinatorService.GetByID(u.Coordinator.ID)).Data[0];
                if (Class.Coordinators != null)
                {
                    foreach (CoordinatorClass coordinatorClass in Class.Coordinators)
                    {
                        if (coordinator.Classes.Where(c => c.ClassID == Class.ID).Any())
                        {
                            hasPermissionToRead = true;
                        }
                    }
                }
            }
            return hasPermissionToRead;
        }
    }
}