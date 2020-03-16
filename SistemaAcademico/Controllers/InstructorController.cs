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
    [Route("instructors")]
    [ApiController]
    public class InstructorController : ControllerBase
    {

        IInstructorService _service;
        IUserService userService;
        ICoordinatorService coordinatorService;
        ICourseService courseService;
        IOwnerService ownerService;
        IStudentService studentService;
        public InstructorController(IInstructorService service, IUserService userService, ICoordinatorService coordinatorService, ICourseService courseService, IOwnerService ownerService, IStudentService studentService)
        {
            this._service = service;
            this.userService = userService;
            this.coordinatorService = coordinatorService;
            this.courseService = courseService;
            this.ownerService = ownerService;
            this.studentService = studentService;
        }

        [Authorize]
        public async Task<object> GetInstructors()
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
        public async Task<object> GetInstructor(int id)
        {
            try
            {
                DataResponse<Instructor> response = await _service.GetByID(id);
                if (await VerifyPermision(response.Data[0]))
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
        public async Task<object> CreateInstructor(Instructor Instructor)
        {
            try
            {
                if (this.GetUserID() == Instructor.UserID) {
                    Response response = await _service.Create(Instructor);
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
        public async Task<object> UpdateInstructor(Instructor Instructor)
        {
            try
            {
                if (this.GetUserID() == Instructor.UserID) {
                    Response response = await _service.Update(Instructor);
                    return this.SendResponse(response);
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
        public async Task<object> DeleteInstructor(int id)
        {
            try
            {
                Instructor instructor = (await _service.GetByID(id)).Data[0];
                if (this.GetUserID() == instructor.UserID)
                {
                    Response response = await _service.Delete(id);
                }
                return Forbid();
            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCode(500).StatusCode;
                return null;
            }
        }

        private async Task<bool> VerifyPermision(Instructor instructor) 
        {
            User user = (await userService.GetByID(this.GetUserID())).Data[0];
            bool isPermited = false;
            if (user.Instructor.ID == instructor.ID)
            {
                isPermited = true;
            }
            foreach (CoordinatorClass coordinatorClass in (await coordinatorService.GetByID(user.Coordinator.ID)).Data[0].Classes)
            {
                if (instructor.Classes.Where(c => c.ClassID == coordinatorClass.ClassID).Count() > 0)
                {
                    isPermited = true;
                }
            }
            foreach (OwnerCourse ownerCourse in (await ownerService.GetByID(user.Owner.ID)).Data[0].Courses)
            {
                foreach (Class @class in (await courseService.GetByID(ownerCourse.CourseID)).Data[0].Classes)
                {
                    if (instructor.Classes.Where(c => c.ClassID == @class.ID).Count() > 0)
                    {
                        isPermited = true;
                    }
                }
            }
            foreach (InstructorClass instructorClass in (await _service.GetByID(user.Instructor.ID)).Data[0].Classes)
            {
                if (instructor.Classes.Where(c => c.ClassID == instructorClass.ClassID).Count() > 0) 
                {
                    isPermited = true;
                }
            }
            foreach (StudentClass StudentClass in (await studentService.GetByID(user.Student.ID)).Data[0].Classes)
            {
                if (instructor.Classes.Where(c => c.ClassID == StudentClass.ClassID).Count() > 0)
                {
                    isPermited = true;
                }
            }
            return isPermited;
        }
    }
}