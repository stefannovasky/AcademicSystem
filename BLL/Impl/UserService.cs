﻿using BLL.Interfaces;
using BLL.Validators;
using DAL.Impl;
using Entities;
using FluentValidation.Results;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class UserService : IUserService
    {
        private UserRepository _userRepo = new UserRepository();

        public async Task<Response> Create(User item)
        {
            Response response = new Response();
            try
            {
                ValidationResult validationResponse = new UserValidator().Validate(item);
                if (!validationResponse.IsValid)
                {
                    response.Success = false;
                    response.ErrorList.Add("Validation error");
                    return response;
                }

                response = await _userRepo.Create(item);
                return response;                
            }
            catch (Exception)
            {
                response.ErrorList.Add("Error on create user");
                response.Success = false; 
                return response; 
            }
        }

        public async Task<Response> Delete(int id)
        {
            try
            {
                Response response = await _userRepo.Delete(id);
                return response;
            }
            catch (Exception)
            {
                Response r = new Response() { Success = false };
                r.ErrorList.Add("Error on delete user");
                return r; 
            }
        }

        public async Task<DataResponse<User>> GetAll()
        {
            try
            {
                DataResponse<User> response = await _userRepo.GetAll();
                return response; 
            }
            catch (Exception)
            {
                DataResponse<User> r = new DataResponse<User>() { Success = false };
                r.ErrorList.Add("Error on read users");
                return r;
            }
        }

        public async Task<DataResponse<User>> GetByID(int id)
        {
            try
            {
                DataResponse<User> response = await _userRepo.GetByID(id);
                return response;
            }
            catch (Exception)
            {
                DataResponse<User> r = new DataResponse<User>() { Success = false };
                r.ErrorList.Add("Error on get user");
                return r;
            }
        }

        public async Task<DataResponse<User>> Update(User item)
        {
            DataResponse<User> response = new DataResponse<User>();
            try
            {
                ValidationResult validationResponse = new UserValidator().Validate(item);
                if (!validationResponse.IsValid)
                {
                    response.Success = false;
                    response.ErrorList.Add("Validation error");
                    return response;
                }

                response = await _userRepo.Update(item);
                return response;
            }
            catch (Exception)
            {
                response.ErrorList.Add("Error on update user");
                response.Success = false;
                return response;
            }
        }
    }
}
