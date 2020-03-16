using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public AttendanceController(IAttendanceService service)
        {
            this._service = service;
        }

        [Authorize]
        public async Task<object> GetAttendances()
        {
            try
            {

                DataResponse<Attendance> response = await _service.GetAll();

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
        public async Task<object> GetAttendance(int id)
        {
            try
            {
                DataResponse<Attendance> response = await _service.GetByID(id);

                //response.Data.ForEach(Attendance => Attendance.Course.Attendances = null);

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

        [HttpPost]
        [Authorize]
        public async Task<object> CreateAttendance(Attendance Attendance)
        {
            try
            {
                Response response = await _service.Create(Attendance);
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
        public async Task<object> UpdateAttendance(Attendance Attendance, int id)
        {
            Attendance.ID = id;
            try
            {
                Response response = await _service.Update(Attendance);
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
        public async Task<object> DeleteAttendance(int id)
        {
            try
            {
                Response response = await _service.Delete(id);

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