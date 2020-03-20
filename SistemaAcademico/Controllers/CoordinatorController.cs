using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AcademicSystemApi.Extensions;
using AcademicSystemApi.Models;
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

        /// <summary>
        ///     Pega um Coordinator através de seu ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetCoordinator(int id)
        {
            try
            {
                if (await CheckPermissionToReadCoordinator(id))
                {
                    DataResponse<Coordinator> response = await _service.GetByID(id);

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

        /// <summary>
        ///     Cria um Coordinator 
        /// </summary>
        /// <param name="Coordinator"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<object> CreateCoordinator(CoordinatorViewModel model)
        {
            Coordinator coordinator = new SimpleAutoMapper<Coordinator>().Map(model);

            try
            {
                if (coordinator.UserID == this.GetUserID())
                {
                    Response response = await _service.Create(coordinator);
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

        /// <summary>
        ///     Altera um Coordinator pelo corpo da requisição através de seu ID 
        /// </summary>
        /// <param name="Coordinator"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<object> UpdateCoordinator(CoordinatorViewModel model, int id)
        {
            Coordinator coordinator = new SimpleAutoMapper<Coordinator>().Map(model);

            coordinator.ID = id;
            try
            {
                if (await this.CheckPermissionToUpdateOrDeleteCoordinator(id))
                {
                    Response response = await _service.Update(coordinator);
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

        /// <summary>
        ///     Deleta um Coordinator através de seu ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<object> DeleteCoordinator(int id)
        {
            try
            {
                if (await CheckPermissionToUpdateOrDeleteCoordinator(id))
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

        /// <summary>
        ///     Verifica se o usuário logado tem permissão para alterar ou deletar determinado Coordinator 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToUpdateOrDeleteCoordinator(int id) 
        {
            try
            {
                Coordinator coordinator = (await _service.GetByID(id)).Data[0];
                if (this.GetUserID() == coordinator.UserID)
                {
                    return true; 
                }
                return false; 
            }
            catch (Exception)
            {
                return false; 
            }
        }


        /// <summary>
        ///     Verifica se o usuário logado tem permissão para ler determinado Coordinator
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToReadCoordinator(int id)
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
                        return true;
                    }
                }

                return false; 
            }
            catch (Exception)
            {
                return false; 
            }
        }
    }
}