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
        public ClassController(IClassService classService, ICourseService courseService, IUserService userService, IOwnerService ownerService, ICoordinatorService coordinatorService)
        {
            this._classService = classService;
            this._courseService = courseService;
            this._userService = userService;
            this._ownerService = ownerService;
            this._coordinatorService = coordinatorService;
        }

        [HttpPost]
        [Authorize]
        public async Task<object> Create(Class Class)
        {
            try
            {
                int coordinatorID = await this.VerifyIfUserIsCoordinatorAndReturnCoordinatorId(_userService);
                if (coordinatorID == 0)
                {
                    return Forbid();
                }


                DataResponse<int> response = await _classService.CreateAndReturnId(Class);

                if (response.HasError())
                {
                    return new
                    {
                        success = response.Success,
                        message = response.GetErrorMessage()
                    };
                }

                await this._courseService.AddClass(new Course() { ID = Class.CourseID }, new Class() { ID = response.Data[0] });

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

        [Authorize]
        public async Task<object> GetAll()
        {
            try
            {
                DataResponse<Class> response = await _classService.GetAll();


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
        public async Task<object> GetClass(int id)
        {
            try
            {
                DataResponse<Class> response = await _classService.GetByID(id);

                // verify 
                Class Class = (await _classService.GetByID(id)).Data[0];
                if (await this.PermissionCheckToReadClass(Class))
                {
                    return this.SendResponse(response);
                }

                return Forbid();
            }
            catch (Exception e)
            {
                return Forbid();
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<object> DeleteClass(int id)
        {
            try
            {
                Response response = await _classService.Delete(id);
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

        [Authorize]
        [HttpPost]
        [Route("student")]
        public async Task<object> AddStudent([FromBody] StudentClass item)
        {
            try
            {
                Response response = await _classService.AddStudent(new Class() { ID = item.ClassID }, new Student() { ID = item.StudentID });
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

        private async Task<bool> PermissionCheckToReadClass(Class Class)
        {
            bool hasPermissionToRead = false;
            int userID = this.GetUserID();
            User u = (await this._userService.GetByID(userID)).Data[0];

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
            if (Class.Instructors != null)
            {
                foreach (InstructorClass instructor in Class.Instructors)
                {
                    if (instructor.InstructorID == u.Instructor.ID)
                    {
                        hasPermissionToRead = true;
                    }
                }
            }
            Coordinator coordinator = (await this._coordinatorService.GetByID(u.Coordinator.ID)).Data[0];
            if (Class.Coordinators != null)
            {
                foreach (CoordinatorClass coordinatorClass in Class.Coordinators)
                {
                    if ((coordinator.Classes.Where(c => c.ClassID == Class.ID).ToList().Count) > 0)
                    {
                        hasPermissionToRead = true;
                    }
                }
            }
            return hasPermissionToRead;
        }
        /*
        private async Task<bool> PermissionCheckToAddClass(Class Class) 
        {
            // Ser um dos coordinator do curso em que a classe está sendo cadastrada 
            //    class Course 
            //     Co
            //Ser owner do curso em que a classe esta sendo cadastrada

            bool hasPermissionToRead = false;
            int userID = this.GetUserID();
            User u = (await this._userService.GetByID(userID)).Data[0];

            if (u.Owner != null)
            {
                Owner owner = (await _ownerService.GetByID(u.Owner.ID)).Data[0];
                foreach (OwnerCourse ownerCourse in owner.Courses)
                {
                    if (ownerCourse.CourseID == Class.CourseID)
                    {
                        hasPermissionToRead = true;
                    }
                }
            }

            if (u.Coordinator != null && Class.Coordinators != null)
            {
                //Coordinator coordinator = (await _coordinatorService.GetByID(u.Coordinator.ID)).Data[0];

                //Course course = (await _courseService.GetByID(Class.CourseID)).Data[0];
                
                foreach (var coordinator in Class.Coordinators)
                {
                    if (coordinator.ClassID == Class.ID)
                    {
                        hasPermissionToRead = true;
                    }
                }
            }

            return true; 
        }*/
    }
}