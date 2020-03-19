using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AcademicSystemApi.Extensions;
using AcademicSystemApi.Models;
using AcademicSystemApi.Services;
using AutoMapper;
using BLL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        /// <summary>
        ///     Pega um user.
        /// </summary>
        /// <param name="id">Id do User a ser pego.</param>
        /// <returns>Retorna um User</returns>
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetByID(int id)
        {
            DataResponse<User> response = await this._service.GetByID(id);

            if (!response.HasError())
            {
                response.Data[0].Password = "";
                response.Data[0].Cpf = "";
                response.Data[0].Rg = "";
                return new
                {
                    user = response.Data[0]
                };
            }

            return new
            {
                message = response.GetErrorMessage()
            };
        }
        /// <summary>
        ///     Método retorna o usuario atual logado.
        /// </summary>
        /// <returns>Retona um User.</returns>
        [HttpGet]
        [Route("profile")]
        [Authorize]
        public async Task<object> Profile()
        {
            int id = this.GetUserID();
            DataResponse<User> response = await this._service.GetByID(id);

            if (!response.HasError())
            {
                response.Data[0].Password = "";

                return this.SendResponse(response);
            }

            return new
            {
                message = response.GetErrorMessage()
            };
        }
        /// <summary>
        ///     Realiza a authenticação do usuario.
        /// </summary>
        /// <param name="model">Recebe o model a partir do corpo da requisisão</param>
        /// <returns>Retorna o usuario logado e o token de sessão</returns>
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

                return this.SendResponse(new { data = response.Data, token = token });
            }

            return new
            {
                message = response.GetErrorMessage()
            };
        }
        /// <summary>
        ///     Metodo cria um novo usuario.
        /// </summary>
        /// <param name="model">Dados para a criação de User passados pelo corpo da requisição</param>
        /// <returns>Retorna um "Ok" caso ocora tudo certo.</returns>
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<object> Register([FromBody] RegisterModel model)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RegisterModel, User>());
            var mapper = new Mapper(config);
            User user = mapper.Map<User>(model);

            Response response = await this._service.Create(user);

            if (!response.HasError())
            {
                return new
                {
                    message = "Ok"
                };
            }

            return new
            {
                message = response.GetErrorMessage()
            };
        }
        /// <summary>
        ///  Metodo deleta um usuario.
        /// </summary>
        /// <param name="id">Id do usuario a ser deletado.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<object> Delete(int id)
        {
            int authenticatedUserId = this.GetUserID();

            if (authenticatedUserId != id)
            {
                return this.Unauthorized();
            }

            Response response = await this._service.Delete(id);

            if (!response.HasError())
            {
                return new
                {
                    message = "Ok"
                };
            }

            return new
            {
                message = response.GetErrorMessage()
            };
        }
        /// <summary>
        ///     Metodo atualiza dados de um usuario.
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<object> Update([FromBody] UpdateUserModel model, int id)
        {
            int authenticatedUserId = this.GetUserID();

            if (authenticatedUserId != id)
            {
                return this.Unauthorized();
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<UpdateUserModel, User>());
            var mapper = new Mapper(config);

            User user = mapper.Map<User>(model);
            user.ID = authenticatedUserId;
            Response response = await this._service.Update(user);

            if (!response.HasError())
            {
                return new
                {
                    message = "Ok"
                };
            }

            return new
            {
                message = response.GetErrorMessage()
            };
        }
    }
}