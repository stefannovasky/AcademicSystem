﻿using System;
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
        public CourseController(ICourseService service, IUserService userService, IOwnerService ownerService, ICoordinatorService coordinatorService, IClassService classService, IInstructorService instructorService, IStudentService studentService)
        {
            this._courseService = service;
            this._userService = userService;
            this._ownerService = ownerService;
            this._coordinatorService = coordinatorService;
            this._classService = classService;
            this._instructorService = instructorService;
            this._studentService = studentService;
        }

        [Authorize]
        public async Task<object> GetCourses()
        {
            try
            {
                return Forbid();
            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCode(500).StatusCode;
                return null;
            }
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetCourse(int id)
        {
            try
            {
                DataResponse<Course> response = await _courseService.GetByID(id);
                if (await this.VerifyPermisionCourse(response.Data[0]))
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
        public async Task<object> CreateCourse(Course Course)
        {
            try
            {
                User user = (await _userService.GetByID(this.GetUserID())).Data[0];
                if (user.Owner != null)
                {
                    Course.Owners.Clear();
                    int id = (await _courseService.CreateAndReturnId(Course)).Data[0];
                    Course = (await _courseService.GetByID(id)).Data[0];
                    return this.SendResponse(await _courseService.AddOwner(Course, user.Owner));
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
        public async Task<object> UpdateCourse(Course Course, int id)
        {
            Course.ID = id;
            try
            {
                User user = (await _userService.GetByID(this.GetUserID())).Data[0];
                if (user.Owner != null)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];
                    foreach (OwnerCourse ownerCourse in Course.Owners)
                    {
                        if (owner.Courses.Contains(ownerCourse))
                        {
                            return this.SendResponse(await _courseService.Update(Course));
                        }
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
        public async Task<object> DeleteCourse(int id)
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
                        return this.SendResponse(await _courseService.Delete(id));
                    }
                }

                Response response = await _courseService.Delete(id);
                return new
                {
                    success = response.Success
                };
            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCode(500).StatusCode;
                return null;
            }
        }


        private async Task<bool> VerifyPermisionCourse(Course course)
        {
            User user = (await _userService.GetByID(this.GetUserID())).Data[0];
            if (user.Owner != null)
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
    }
}