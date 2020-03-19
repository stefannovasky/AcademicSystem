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


        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetCourse(int id)
        {
            try
            {
                DataResponse<Course> response = await _courseService.GetByID(id);
                if (await this.PermissionCheckToReadCourse(response.Data[0]))
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
        public async Task<object> CreateCourse(Course course)
        {
            try
            {
                if (await this.CheckPermissionToCreateCourse(course))
                {
                    User user = (await _userService.GetByID(this.GetUserID())).Data[0];

                    course.Owners.Clear();
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

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<object> UpdateCourse(Course course, int id)
        {
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

        [HttpPost]
        [Route("subject")]
        [Authorize]
        public async Task<object> AddSubject(int courseID, int subjectID)
        {
            try
            {
                if (await this.PermissionCheckToAddSubject(courseID))
                {
                    return await _courseService.AddSubject((await _courseService.GetByID(courseID)).Data[0], (await _subjectService.GetByID(subjectID)).Data[0]);
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
        [Route("owner")]
        [Authorize]
        public async Task<object> AddOwner(OwnerCourse ownerCourse)
        {
            try
            {
                if (await this.PermissionCheckToAddOwner(ownerCourse))
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

        private async Task<bool> VerifyPermisionCourse(Course course)
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
            if (user.Coordinator != null)
            {
                Coordinator coordinator = (await _coordinatorService.GetByID(user.Coordinator.ID)).Data[0];
                foreach (CoordinatorClass coordinatorClass in coordinator.Classes)
                {
                    if (course.Classes.Where(c => c.ID == coordinatorClass.ClassID).Any())
                    {
                        return true;
                    }
                }
            }
            if (user.Instructor != null)
            {
                Instructor instructor = (await _instructorService.GetByID(user.Instructor.ID)).Data[0];
                foreach (InstructorClass instructorClass in instructor.Classes)
                {
                    if (course.Classes.Where(c => c.ID == instructorClass.ClassID).Any())
                    {
                        return true;
                    }
                }
            }
            if (user.Student != null)
            {
                Student student = (await _studentService.GetByID(user.Student.ID)).Data[0];
                foreach (StudentClass studentClass in student.Classes)
                {
                    if (course.Classes.Where(c => c.ID == studentClass.ClassID).Any())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

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

        private async Task<bool> PermissionCheckToAddSubject(int courseID)
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

        private async Task<bool> PermissionCheckToAddOwner(OwnerCourse ownerCourse)
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

        private async Task<bool> PermissionCheckToReadCourse(Course c)
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