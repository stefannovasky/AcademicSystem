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
    [Route("courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {        
        ICourseService _courseService;
        IUserService _userService;
        IOwnerService _ownerService;
        IStudentService _studentService;
        IClassService _classService;
        ICoordinatorService _coordinatorService;
        public CourseController(ICourseService service, IUserService userService, IOwnerService ownerService, IStudentService studentService, IClassService classService, ICoordinatorService coordinatorService)
        {
            this._courseService = service;
            this._userService = userService;
            this._ownerService = ownerService;
            this._studentService = studentService;
            this._classService = classService;
            this._coordinatorService = coordinatorService;
        }
        /*
        [Authorize]
        public async Task<object> GetCourses()
        {                                                                                                                     
            try
            {
                DataResponse<Course> response = await _courseService.GetAll();
                
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

        private async Task<bool> PermissionCheckToReadCourse(Course c)
        {
            User u = (await _userService.GetByID(this.GetUserID())).Data[0];

            // verify 
            bool hasPermissionToRead = false;
            if (u.Owner != null)
            {
                Owner o = (await _ownerService.GetByID(u.Owner.ID)).Data[0];
                foreach (OwnerCourse ownerCourse in o.Courses)
                {
                    if ((c.Owners.Where(owner => owner.CourseID == ownerCourse.OwnerID).ToList()).Count > 0)
                    {
                        hasPermissionToRead = true;
                    }
                }
            }
            if (u.Student != null)
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
            if (u.Coordinator != null)
            {
                Coordinator coordinator = (await this._coordinatorService.GetByID(u.Coordinator.ID)).Data[0];
                foreach (CoordinatorClass coordinatorClass in coordinator.Classes)
                {
                    Class Class = (await this._classService.GetByID(coordinatorClass.ClassID)).Data[0];
                    if (Class.Coordinators.Where(c => c.CoordinatorID == coordinator.ID).ToList().Count > 0)
                    {
                        hasPermissionToRead = true;
                    }
                }
            }

            return hasPermissionToRead;
        }


        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetCourse(int id)
        {
            try
            {
                DataResponse<Course> response = await _courseService.GetByID(id);

                if (response.HasError())
                {
                    return this.SendResponse(response);
                }

                // verify
                if (await this.PermissionCheckToReadCourse(response.Data[0]))
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
        public async Task<object> CreateCourse(Course Course)
        {
            try
            {
                int ownerId = await this.VerifyIfUserIsOwnerAndReturnOwnerId(_userService);
                if (ownerId == 0)
                {
                    return Forbid(); 
                }

                
                DataResponse<int> response = await _courseService.CreateAndReturnId(Course);

                if (response.HasError())
                {
                    return new
                    {
                        success = response.Success,
                        message = response.GetErrorMessage()
                    };
                }

                await this._ownerService.AddCourse(new Owner() { ID = ownerId }, new Course() { ID = response.Data[0] });

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
        public async Task<object> UpdateCourse(Course Course, int id)
        {
            try
            {
                int ownerID = await this.VerifyIfUserIsOwnerAndReturnOwnerId(_userService);
                if (ownerID == 0)
                {
                    return Forbid();
                }
                Course.ID = id; 
                Response response = await _courseService.Update(Course);
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
        public async Task<object> DeleteCourse(int id)
        {
            try
            {
                int ownerID = await this.VerifyIfUserIsOwnerAndReturnOwnerId(_userService);
                if (ownerID == 0)
                {
                    return Forbid();
                }

                Response response = await _courseService.Delete(id);
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