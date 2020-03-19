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
    [Route("subjects")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        ISubjectService _service;
        IUserService _userService;
        IInstructorService _instructorService;
        IStudentService _studentService;
        ICoordinatorService _coordinatorService;
        IClassService _classService;
        ICourseService _courseService;
        IOwnerService _ownerService;
        public SubjectController(ISubjectService service, IUserService userService, IInstructorService instructorService, IStudentService studentService, ICoordinatorService coordinatorService, IClassService classService, ICourseService courseService, IOwnerService ownerService)
        {
            this._service = service;
            this._userService = userService;
            this._instructorService = instructorService;
            this._studentService = studentService;
            this._coordinatorService = coordinatorService;
            this._classService = classService;
            this._courseService = courseService;
            this._ownerService = ownerService;
        }
       
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetSubject(int id)
        {
            try
            {
                DataResponse<Subject> response = await _service.GetByID(id);

                if (response.HasError())
                {
                    return response;
                }

                if (await this.CheckPermissionToReadSubject(response.Data[0]))
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
        public async Task<object> CreateSubject(Subject Subject)
        {
            try
            {
                if (await CheckPermissionToCreateUpdateSubject(Subject))
                {
                    Response response = await _service.Create(Subject);
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

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<object> UpdateSubject(Subject Subject, int id)
        {
            Subject.ID = id;
            try
            {
                if (await CheckPermissionToCreateUpdateSubject(Subject))
                {
                    Response response = await _service.Update(Subject);
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

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<object> DeleteSubject(int id)
        {
            try
            {
                if (await CheckPermissionToDeleteSubject(id))
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

        private async Task<bool> CheckPermissionToCreateUpdateSubject(Subject subject)
        {
            User user = (await this._userService.GetByID(this.GetUserID())).Data[0];
            if (user.Owner != null && user.Owner.IsActive)
            {
                Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];

                foreach (OwnerCourse oc in owner.Courses)
                {
                    Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                    if (course.Classes.Where(c => c.ID == subject.ID).Any())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private async Task<bool> CheckPermissionToDeleteSubject(int id)
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

        private async Task<bool> CheckPermissionToReadSubject(Subject subject)
        {

            User user = (await this._userService.GetByID(this.GetUserID())).Data[0];
            if (user.Instructor != null && user.Instructor.IsActive)
            {
                Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];
                if (subject.Instructors != null && user.Instructor.IsActive)
                {
                    foreach (SubjectInstructor subjectInstructor in subject.Instructors)
                    {
                        if (instructor.Subjects.Where(s => s.InstructorID == subjectInstructor.InstructorID).Any())
                        {
                            return true;
                        }
                    }
                }
            }

            if (user.Student != null && user.Student.IsActive)
            {
                Student student = (await this._studentService.GetByID(user.Student.ID)).Data[0];
                foreach (Class Class in subject.Classes)
                {
                    if (student.Classes.Where(c => c.ClassID == Class.ID).Any())
                    {
                        return true;
                    }
                }
            }

            if (user.Coordinator != null && user.Coordinator.IsActive)
            {
                Coordinator coordinator = (await this._coordinatorService.GetByID(user.Coordinator.ID)).Data[0];

                foreach (CoordinatorClass coordinatorClass in coordinator.Classes)
                {
                    Class Class = (await this._classService.GetByID(coordinatorClass.ClassID)).Data[0];
                    if (Class.CourseID == subject.CourseID)
                    {
                        return true;
                    }
                }
            }

            if (user.Owner != null && user.Owner.IsActive)
            {
                Owner owner = (await this._ownerService.GetByID(user.Owner.ID)).Data[0];

                foreach (OwnerCourse ownerCourse in owner.Courses)
                {
                    Course course = (await this._courseService.GetByID(ownerCourse.CourseID)).Data[0];

                    if (course.Subjects.Where(s => s.ID == subject.ID).Any())
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}