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
    [Route("students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        IStudentService _service;
        IUserService userService;
        IInstructorService InstructorService;
        ICoordinatorService CoordinatorService;
        IOwnerService OwnerService;
        ICourseService CourseService;
        public StudentController(IStudentService service, IUserService userService, IInstructorService InstructorService, ICoordinatorService coordinatorService, IOwnerService ownerService, ICourseService courseService)
        {
            this._service = service;
            this.userService = userService;
            this.InstructorService = InstructorService;
            this.CoordinatorService = coordinatorService;
            this.OwnerService = ownerService;
            this.CourseService = courseService;
        }


        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<object> GetStudent(int id)
        {
            try
            {
                DataResponse<Student> response = (await _service.GetByID(id));

                if (response.HasError())
                {
                    return response;
                }

                if (await CheckPermisionToGetStudent(response.Data[0]))
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

        [Authorize]
        [HttpPost]
        public async Task<object> CreateStudent(Student student)
        {
            try
            {
                if (await CheckPermissionToCreateUpdateStudent(student))
                {
                    Response response = await _service.Create(student);
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

        [Authorize]
        [HttpPut]
        [Route("{id}")]
        public async Task<object> UpdateStudent(Student student, int id)
        {
            student.ID = id;
            try
            {
                if (await CheckPermissionToCreateUpdateStudent(student))
                {
                    Response response = await _service.Update(student);
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

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<object> DeleteStudent(int id)
        {
            try
            {
                if (await CheckPermissionToDeleteStudent(id))
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


        private async Task<bool> CheckPermissionToCreateUpdateStudent(Student student)
        {
            User user = (await userService.GetByID(this.GetUserID())).Data[0];
            if (student.UserID == user.ID)
            {
                return true;
            }
            return false;
        }

        private async Task<bool> CheckPermissionToDeleteStudent(int id)
        {
            User user = (await userService.GetByID(this.GetUserID())).Data[0];
            Student student = (await _service.GetByID(id)).Data[0];
            if (student.UserID == user.ID)
            {
                return true;
            }
            return false;
        }

        private async Task<bool> CheckPermisionToGetStudent(Student student)
        {
            User user = (await userService.GetByID(this.GetUserID())).Data[0];
            if (user.Student != null && user.Student.IsActive && user.Student.ID == student.ID )
            {
                return true;
            }
            if (user.Instructor != null && user.Instructor.IsActive)
            {
                Instructor instructor = (await InstructorService.GetByID(user.Instructor.ID)).Data[0];
                foreach (InstructorClass instructorClass in instructor.Classes)
                {
                    if (student.Classes.Where(ic => ic.ClassID == instructorClass.ClassID).Any())
                    {
                        return true;
                    }
                }
            }
            if (user.Coordinator != null && user.Coordinator.IsActive)
            {
                Coordinator Coordinator = (await CoordinatorService.GetByID(user.Coordinator.ID)).Data[0];

                foreach (CoordinatorClass CoordinatorClass in Coordinator.Classes)
                {
                    if (student.Classes.Where(ic => ic.ClassID == CoordinatorClass.ClassID).Any())
                    {
                        return true;
                    }
                }
            }
            if (user.Owner != null && user.Owner.IsActive)
            {
                Owner Owner = (await OwnerService.GetByID(user.Owner.ID)).Data[0];
                foreach (OwnerCourse ownerCourse in Owner.Courses)
                {
                    Course course = (await CourseService.GetByID(ownerCourse.CourseID)).Data[0];
                    foreach (Class @class in course.Classes)
                    {
                        if (student.Classes.Where(sc => sc.ClassID == @class.ID).Any())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}