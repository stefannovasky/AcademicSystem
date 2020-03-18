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
    [Route("coordinators")]
    [ApiController]
    public class CoordinatorController : ControllerBase
    {

        ICoordinatorService _service;
        IUserService userService;
        public CoordinatorController(ICoordinatorService service, IUserService userService)
        {
            this._service = service;
            this.userService = userService;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetCoordinator(int id)
        {
            try
            {
                DataResponse<Coordinator> response = await _service.GetByID(id);
                User user = (await userService.GetByID(this.GetUserID())).Data[0];
                Coordinator coordinator = (await _service.GetByID(id)).Data[0];
                foreach (CoordinatorClass coordinatorClass in response.Data[0].Classes)
                {
                    if (coordinator.Classes.Where(c => c.ClassID == coordinatorClass.ClassID).Any())
                    {
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

        [HttpPost]
        [Authorize]
        public async Task<object> CreateCoordinator(Coordinator Coordinator)
        {
            try
            {
                if (Coordinator.UserID == this.GetUserID())
                {
                    Response response = await _service.Create(Coordinator);
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
        public async Task<object> UpdateCoordinator(Coordinator Coordinator, int id)
        {
            Coordinator.ID = id;
            try
            {
                if (Coordinator.UserID == this.GetUserID())
                {
                    Response response = await _service.Update(Coordinator);
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
        public async Task<object> DeleteCoordinator(int id)
        {
            try
            {
                Coordinator coordinator = (await _service.GetByID(id)).Data[0];
                if (this.GetUserID() == coordinator.UserID) {
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
    }
}