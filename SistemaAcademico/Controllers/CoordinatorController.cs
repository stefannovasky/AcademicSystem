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
    [Route("coordinators")]
    [ApiController]
    public class CoordinatorController : ControllerBase
    {

        ICoordinatorService _service;
        public CoordinatorController(ICoordinatorService service)
        {
            this._service = service;
        }

        [Authorize]
        public async Task<object> GetCoordinators()
        {
            try
            {
                DataResponse<Coordinator> response = await _service.GetAll();
                foreach (Coordinator Coordinator in response.Data)
                {
                    if (Coordinator.User != null)
                        Coordinator.User.Coordinator = null;
                }
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
        public async Task<object> GetCoordinator(int id)
        {
            try
            {
                DataResponse<Coordinator> response = await _service.GetByID(id);
                response.Data[0].User.Coordinator = null;
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
        public async Task<object> CreateCoordinator(Coordinator Coordinator)
        {
            try
            {
                Response response = await _service.Create(Coordinator);
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
        public async Task<object> UpdateCoordinator(Coordinator Coordinator)
        {
            try
            {
                Response response = await _service.Update(Coordinator);
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
        public async Task<object> DeleteCoordinator(int id)
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