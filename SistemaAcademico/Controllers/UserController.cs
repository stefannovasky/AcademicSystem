using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicSystemApi.Models;
using AcademicSystemApi.Services;
using BLL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace AcademicSystemApi.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _service;
        public UserController(IUserService service)
        {
            this._service = service;
        }

        [HttpPost]
        [Route("auth")]
        [AllowAnonymous]
        public async Task<object> Authenticate([FromBody] AuthenticateModel model) 
        {
            User u = new User() { Email = model.Email, Password = model.Password };
            DataResponse<User> response = await this._service.Authenticate(u);

            if (!response.HasError())
            {
                string token = TokenService.GenerateToken(response.Data[0]);
                response.Data[0].Password = "";
                return new
                {
                    user = response.Data[0], 
                    token = token
                };
            }

            return new
            {
                message = response.GetErrorMessage()
            };
        }


        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<object> GetAll()
        {
            DataResponse<User> response = await _service.GetAll();
            return response; 
        }
    }
}