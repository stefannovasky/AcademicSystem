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
    public class InstructorService : IInstructorService
    {
        private InstructorRepository _InstructorRepo = new InstructorRepository();
        public async Task<Response> Create(Instructor item)
        {
            Response response = new Response();
            try
            {
                response = await _InstructorRepo.Create(item);
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorList.Add("Error on create Instructor");
                response.Success = false;
                return response;
            }
        }

        public async Task<Response> Delete(int id)
        {
            try
            {
                Response response = await _InstructorRepo.Delete(id);
                return response;
            }
            catch (Exception)
            {
                Response r = new Response() { Success = false };
                r.ErrorList.Add("Error on delete Instructor");
                return r;
            }
        }

        public async Task<DataResponse<Instructor>> GetAll()
        {
            try
            {
                DataResponse<Instructor> response = await _InstructorRepo.GetAll();
                return response;
            }
            catch (Exception)
            {
                DataResponse<Instructor> r = new DataResponse<Instructor>() { Success = false };
                r.ErrorList.Add("Error on read Instructors");
                return r;
            }
        }

        public async Task<DataResponse<Instructor>> GetByID(int id)
        {
            try
            {
                DataResponse<Instructor> response = await _InstructorRepo.GetByID(id);
                return response;
            }
            catch (Exception)
            {
                DataResponse<Instructor> r = new DataResponse<Instructor>() { Success = false };
                r.ErrorList.Add("Error on get Instructor");
                return r;
            }
        }

        public async Task<DataResponse<Instructor>> Update(Instructor item)
        {
            DataResponse<Instructor> response = new DataResponse<Instructor>();
            try
            {
                response = await _InstructorRepo.Update(item);
                return response;
            }
            catch (Exception)
            {
                response.ErrorList.Add("Error on update Instructor");
                response.Success = false;
                return response;
            }
        }

        public async Task<Response> AddSubject(Instructor instructor, Subject subject) 
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
                response = await _InstructorRepo.AddSubject(instructor, subject);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while adding Subject into instryuctor");
                return response;
            }
        }

        public async Task<Response> AddClass(Instructor instructor, Class Class)
        {
            Response response = new Response();
            try
            {
                response = await _InstructorRepo.AddClass(instructor, Class);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while adding Class in Instructor");
                return response;
            }
        }
    }
}
