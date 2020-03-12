using BLL.Interfaces;
using BLL.Validators;
using DAL.Impl;
using DAL.Interfaces;
using Entities;
using FluentValidation.Results;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class CourseService : ICourseService
    {
        private ICourseRepository _repository;

        public CourseService(ICourseRepository repository)
        {
            this._repository = repository;
        }

        public async Task<Response> Create(Course item)
        {
            Response response = new Response();
            try
            {
                ValidationResult validationResponse = await new CourseValidator().ValidateAsync(item);
                if (!validationResponse.IsValid)
                {
                    response.Success = false;
                    response.ErrorList.Add("Validation Error");
                    return response;
                }
                response = await _repository.Create(item);
                return response;
            }
            catch (Exception e)
            {
                response.ErrorList.Add("Error while creating Service.");
                response.Success = false;
                return response;
            }
        }

        public async Task<Response> Delete(int id)
        {
            Response response = new Response();
            try
            {
                response = await _repository.Delete(id);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Erro while deleting Course");
                return response;
            }
        }

        public async Task<DataResponse<Course>> GetAll()
        {
            DataResponse<Course> response = new DataResponse<Course>();
            try
            {
                response = await _repository.GetAll();
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Erro inesperado");
                return response;
            }
        }

        public async Task<DataResponse<Course>> GetByID(int id)
        {
            DataResponse<Course> response = new DataResponse<Course>();
            try
            {
                response = await _repository.GetByID(id);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Erro inesperado");
                return response;
            }
        }

        public async Task<DataResponse<Course>> Update(Course item)
        {
            DataResponse<Course> response = new DataResponse<Course>();
            try
            {
                ValidationResult validationResponse = await new CourseValidator().ValidateAsync(item);
                if (!validationResponse.IsValid)
                {
                    response.Success = false;
                    response.ErrorList.Add("Validation Error");
                    return response;
                }
                response = await _repository.Update(item);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Erro inesperado");
                return response;
            }
        }

        public async Task<Response> AddClass(Course course, Class Class)
        {
            Response response = new Response();
            try
            {
                response = await _repository.AddClass(course, Class);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while adding Class to Course");
                return response;
            }
        }

        public async Task<Response> AddSubject(Course course, Subject subject)
        {
            Response response = new Response();
            try
            {
                ValidationResult validationResponse = await new SubjectValidator().ValidateAsync(subject);
                if (!validationResponse.IsValid)
                {
                    response.Success = false;
                    response.ErrorList.Add("Validation Error");
                    return response;
                }
                response = await _repository.AddSubject(course, subject);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while adding sybject to Course.");
                return response;
            }
        }

        public async Task<Response> AddOwner(Course course, Owner owner)
        {
            Response response = new Response();
            try
            {
                response = await _repository.AddOwner(course, owner);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while adding owner in Course.");
                return response;
            }
        }
    }
}
