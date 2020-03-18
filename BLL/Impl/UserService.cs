using BLL.Interfaces;
using BLL.Validators;
using DAL.Impl;
using DAL.Interfaces;
using Entities;
using FluentValidation.Results;
using log4net;
using Shared;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepo;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public UserService(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }

        public async Task<DataResponse<User>> Authenticate(User user)
        {
            DataResponse<User> r = new DataResponse<User>();

            try
            {
                DataResponse<User> getUserResponse = await _userRepo.GetByEmail(user.Email);

                if (getUserResponse.HasError())
                {
                    return getUserResponse; 
                }

                User findedUser = getUserResponse.Data[0];

                if (!await new HashUtils().CompareHash(user.Password, findedUser.Password))
                {
                    r.Success = false;
                    r.ErrorList.Add("Invalid password");
                    return r; 
                }

                r.Data.Add(findedUser);
                r.Success = true;
                return r; 
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());

                r.Success = false;
                r.ErrorList.Add("Error on user authentication");
                return r; 
            }
        }

        public async Task<Response> Create(User item)
        {
            Response response = new Response();
            try
            {
                ValidationResult validationResponse = await new UserValidator().ValidateAsync(item);
                if (!validationResponse.IsValid)
                {
                    response.Success = false;
                    response.ErrorList.Add("Validation error");
                    return response;
                }

                item.Password = await new HashUtils().HashString(item.Password);
                response = await _userRepo.Create(item);
                return response;                
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
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
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
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
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
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
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
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
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.ErrorList.Add("Error on update user");
                response.Success = false;
                return response;
            }
        }
    }
}
