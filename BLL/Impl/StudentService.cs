using BLL.Interfaces;
using DAL.Impl;
using DAL.Interfaces;
using Entities;
using log4net;
using Shared;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class StudentService : IStudentService
    {
        private IStudentRepository _studentRepo;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepo = studentRepository;
        }
        public async Task<Response> Create(Student item)
        {
            Response response = new Response();
            try
            {
                response = await _studentRepo.Create(item);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.ErrorList.Add("Error on create student");
                response.Success = false;
                return response;
            }
        }

        public async Task<Response> Delete(int id)
        {
            try
            {
                Response response = await _studentRepo.Delete(id);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                Response r = new Response() { Success = false };
                r.ErrorList.Add("Error on delete Student");
                return r;
            }
        }

        public async Task<DataResponse<Student>> GetAll()
        {
            try
            {
                DataResponse<Student> response = await _studentRepo.GetAll();
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                DataResponse<Student> r = new DataResponse<Student>() { Success = false };
                r.ErrorList.Add("Error on read Students");
                return r;
            }
        }

        public async Task<DataResponse<Student>> GetByID(int id)
        {
            try
            {
                DataResponse<Student> response = await _studentRepo.GetByID(id);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                DataResponse<Student> r = new DataResponse<Student>() { Success = false };
                r.ErrorList.Add("Error on get Student");
                return r;
            }
        }

        public async Task<DataResponse<Student>> Update(Student item)
        {
            DataResponse<Student> response = new DataResponse<Student>();
            try
            {
                response = await _studentRepo.Update(item);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.ErrorList.Add("Error on update Student");
                response.Success = false;
                return response;
            }
        }
    }
}
