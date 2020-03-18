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
        public async Task<object> GetStudents()
        {
            try
            {
                return Forbid();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<object> GetStudent(int id)
        {
            try
            {
                bool isPermited = false;
                DataResponse<Student> studentResponse = (await _service.GetByID(id));
                if (studentResponse.Success)
                {
                    Student student = studentResponse.Data[0];
                    isPermited = await PermisionCheckStudentInClassViewOrUpdate(student);
                }
                if (isPermited)
                {
                    studentResponse.Data[0].User.Student = null;
                    return this.SendResponse(studentResponse);
                }
                return Forbid();
            } catch (Exception e)
            {
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<object> CreateStudent(Student student)
        {
            try
            {
                User user = (await userService.GetByID(this.GetUserID())).Data[0];
                if (student.UserID == user.ID)
                {
                    Response response = await _service.Create(student);
                    return this.SendResponse(response);
                }
                return Forbid();
            }
            catch (Exception e)
            {
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
                User user = (await userService.GetByID(this.GetUserID())).Data[0];
                if (student.UserID == user.ID)
                {
                    Response response = await _service.Update(student);
                    return this.SendResponse(response);
                }
                return Forbid();
            }
            catch (Exception e)
            {
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
                User user = (await userService.GetByID(this.GetUserID())).Data[0];
                Student student = (await _service.GetByID(id)).Data[0];
                if (student.UserID == user.ID)
                {
                    Response response = await _service.Delete(id);
                    return this.SendResponse(response);
                }
                return Forbid();
            }
            catch (Exception e)
            {
                return null;
            }
        }


        private async Task<bool> PermisionCheckStudentInClassViewOrUpdate(Student student)
        {
            User user = (await userService.GetByID(this.GetUserID())).Data[0];
            if (user.Student != null && user.Student.ID == student.ID)
            {
                return true;
            }
            if (user.Instructor != null)
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
            if (user.Coordinator != null)
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
            if (user.Owner != null)
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