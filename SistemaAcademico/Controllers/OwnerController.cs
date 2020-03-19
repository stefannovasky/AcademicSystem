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
    [Route("owners")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        IOwnerService _service;
        IUserService _userService;
        IOwnerService _ownerService;
        public OwnerController(IOwnerService service, IUserService userService, IOwnerService ownerService)
        {
            this._service = service;
            this._userService = userService;
            this._ownerService = ownerService;
        }
        /// <summary>
        /// Metodo pega um owner.
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetOwner(int id)
        {
            try
            {
                DataResponse<Owner> response = await _service.GetByID(id);

                if (response.HasError())
                {
                    return response;
                }

                if (await CheckPermissionToGetOwner(response.Data[0]))
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
        /// <summary>
        /// Metodo cria um owner.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<object> CreateOwner(Owner Owner)
        {
            try
            {
                if(await CheckPermissionToCreateUpdateOwner(Owner))
                {
                    Response response = await _service.Create(Owner);
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
        /// Metodo Atualiza um owner.
        /// </summary>
        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<object> UpdateOwner(Owner Owner, int id)
        {
            Owner.ID = id;
            try
            {
                if (await CheckPermissionToCreateUpdateOwner(Owner);
                {
                    Response response = await _service.Update(Owner);
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
        /// Metodo deleta um owner.
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<object> DeleteOwner(int id)
        {
            try
            {
                if (await CheckPermissionToDeleteOwner(id))
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

        /// <summary>
        /// Metodo checa permiçoes de criação e atualização de um owner.
        /// </summary>
        private async Task<bool> CheckPermissionToCreateUpdateOwner(Owner owner)
        {
            if (this.GetUserID() == (await _service.GetByID(owner.ID)).Data[0].UserID)
            {
                return true;
            }
            return false; ;
        }
        /// <summary>
        /// Checa as permissoes de Delete de um owner.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<bool> CheckPermissionToDeleteOwner(int id)
        {
            if (this.GetUserID() == (await _service.GetByID(id)).Data[0].UserID)
            {
                return true;
            }
            return false; ;
        }
        /// <summary>
        /// Checa permissoes de pegar um owner.
        /// </summary>
        private async Task<bool> CheckPermissionToGetOwner(Owner owner)
        {
            User user = (await _userService.GetByID(this.GetUserID())).Data[0];
            if (user.Owner != null && user.Owner.IsActive)
            {
                if (user.Owner.ID == owner.ID)
                {
                    return true;
                }
                Owner userOwner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];
                foreach (OwnerCourse ownerCourse in owner.Courses)
                {
                    if (userOwner.Courses.Where(c => c.CourseID == ownerCourse.CourseID).Any())
                    {
                        return true;
                    }
                }
            }
            return true;
        }
    }
}