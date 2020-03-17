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
        /*
        [Authorize]
        public async Task<object> GetSubjects()
        {
            try
            {
                DataResponse<Subject> response = await _service.GetAll();

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
        private async Task<bool> PermissionCheckToReadSubject(Subject subject)
        {

            User user = (await this._userService.GetByID(this.GetUserID())).Data[0];
            if (user.Instructor != null)
            {
                Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];
                if (subject.Instructors != null)
                {
                    foreach (SubjectInstructor subjectInstructor in subject.Instructors)
                    {
                        if (instructor.Subjects.Where(s => s.InstructorID == subjectInstructor.InstructorID).ToList().Count > 0) 
                        {
                            return true;
                        }
                    }
                }
            }

            if (user.Student != null)
            {
                Student student = (await this._studentService.GetByID(user.Student.ID)).Data[0];
                foreach (Class Class in subject.Classes)
                {
                    if (student.Classes.Where(c => c.ClassID == Class.ID).ToList().Count > 0)
                    {
                        return true;
                    }
                }
            }

            if (user.Coordinator != null)
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

            return false;
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

                if (await this.PermissionCheckToReadSubject(response.Data[0]))
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
        public async Task<object> CreateSubject(Subject Subject)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Owner != null)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];

                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == Subject.CourseID).Any())
                        {
                            Response response = await _service.Create(Subject);
                            return this.SendResponse(response);
                        }
                    }
                }

                return Forbid();
            }
            catch (Exception e)
            {
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
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Owner != null)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];

                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == Subject.CourseID).Any())
                        {
                            Response response = await _service.Update(Subject);
                            return this.SendResponse(response);
                        }
                    }
                }

                return Forbid();
            }
            catch (Exception e)
            {
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
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Owner != null)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];

                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == id).Any())
                        {
                            Response response = await _service.Delete(id);
                            return this.SendResponse(response);
                        }
                    }
                }

                return Forbid();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}