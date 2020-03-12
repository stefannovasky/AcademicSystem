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
    public class ClassService : IClassService
    {
        private IClassRepository _repository;


        public ClassService(IClassRepository repository)
        {
            this._repository = repository;
        }

        public async Task<Response> Create(Class item)
        {
            Response response = new Response();
            try
            {
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
                response.ErrorList.Add("Erro while deleting Class");
                return response;
            }
        }

        public async Task<DataResponse<Class>> GetAll()
        {
            DataResponse<Class> response = new DataResponse<Class>();
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

        public async Task<DataResponse<Class>> GetByID(int id)
        {
            DataResponse<Class> response = new DataResponse<Class>();
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

        public async Task<DataResponse<Class>> Update(Class item)
        {
            DataResponse<Class> response = new DataResponse<Class>();
            try
            {
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

        public async Task<Response> AddInstructor(Class Class, Instructor instructor)
        {
            Response response = new Response();
            try
            {
                response = await _repository.AddInstructor(Class, instructor);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while adding instructor in class.");
                return response;
            }
        }

        public async Task<Response> AddCoordinator(Class Class, Coordinator Coordinator)
        {
            Response response = new Response();
            try
            {
                response = await _repository.AddCoordinator(Class, Coordinator);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while adding Coordinator in class.");
                return response;
            }
        }

        public async Task<Response> AddEvaluation(Class Class, Evaluation Evaluation)
        {
            Response response = new Response();
            try
            {
                response = await _repository.AddEvaluation(Class, Evaluation);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while adding Evaluation in class.");
                return response;
            }
        }

        public async Task<Response> AddAttendance(Class Class, Attendance Attendance)
        {
            Response response = new Response();
            try
            {
                response = await _repository.AddAttendance(Class, Attendance);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while adding Attendance in class.");
                return response;
            }
        }

        public async Task<Response> AddStudent(Class Class, Student Student)
        {
            Response response = new Response();
            try
            {
                response = await _repository.AddStudent(Class, Student);
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while adding Student in class.");
                return response;
            }
        }
    }
}
