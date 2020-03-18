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
    [Route("attendances")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        IAttendanceService _service;
        IUserService _userService; 
        ICoordinatorService _coordinatorService;
        IClassService _classService;
        IStudentService _studentService;
        IInstructorService _instructorService; 

        public AttendanceController(IAttendanceService service, IUserService userService, ICoordinatorService coordinatorService, IClassService classService, IStudentService studentService, IInstructorService instructorService)
        {
            this._service = service;
            this._userService = userService;
            this._coordinatorService = coordinatorService;
            this._classService = classService;
            this._studentService = studentService;
            this._instructorService = instructorService;
        }

        private async Task<bool> PermissionCheckToReadAttendance(Attendance attendance)
        {
            bool hasPermissionToRead = false;

            User user = (await this._userService.GetByID(this.GetUserID())).Data[0];
            //Ser Coordinator/ Instructor da classe que a attendance foi registrada
            //Ser o aluno registrado pelo atendance

            if (user.Coordinator != null && user.IsActive)
            {
                Coordinator coordinator = (await this._coordinatorService.GetByID(user.Coordinator.ID)).Data[0];
                if (coordinator.Classes.Where(c => c.ClassID == attendance.ClassID).Any())
                {
                    hasPermissionToRead = true;
                }
            }
            if (user.Instructor != null && user.IsActive)
            {
                Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];
                if (instructor.Classes.Where(i => i.ClassID == attendance.ClassID).Any()) 
                {
                    hasPermissionToRead = true;
                }
            }
            if (user.Student != null && user.IsActive)
            {
                if (user.Student.ID == attendance.StudentID)
                {
                    hasPermissionToRead = true; 
                }
            }

            return hasPermissionToRead;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetAttendance(int id)
        {
            try
            {
                DataResponse<Attendance> response = await _service.GetByID(id);

                if (response.HasError())
                {
                    return response; 
                }

                if (await this.PermissionCheckToReadAttendance(response.Data[0]))
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
        public async Task<object> CreateAttendance(Attendance Attendance)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Instructor != null && user.IsActive)
                {
                    Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];

                    if (instructor.Classes.Where(c => c.ClassID == Attendance.ClassID).Any())
                    {
                        Response response = await _service.Create(Attendance);
                        return this.SendResponse(response);
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


        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<object> UpdateAttendance(Attendance Attendance, int id)
        {
            Attendance.ID = id;
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Instructor != null && user.IsActive)
                {
                    Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];

                    if (instructor.Classes.Where(c => c.ClassID == Attendance.ClassID).Any())
                    {
                        Response response = await _service.Update(Attendance);
                        return this.SendResponse(response);
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
        public async Task<object> DeleteAttendance(int id)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Instructor != null && user.IsActive)
                {
                    Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];
                    Attendance Attendance = (await this._service.GetByID(id)).Data[0];
                    if (instructor.Classes.Where(c => c.ClassID == Attendance.ClassID).Any())
                    {
                        Response response = await _service.Delete(id);
                        return this.SendResponse(response);
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
    }
}