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
    class CoordinatorService : ICoordinatorService
    {
        private ICoordinatorRepository _CoordinatorRepo;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CoordinatorService(ICoordinatorRepository repository)
        {
            this._CoordinatorRepo = repository;
        }

        public async Task<Response> Create(Coordinator item)
        {
            Response response = new Response();
            try
            {
                response = await _CoordinatorRepo.Create(item);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.ErrorList.Add("Error on create Coordinator");
                response.Success = false;
                return response;
            }
        }

        public async Task<Response> Delete(int id)
        {
            try
            {
                Response response = await _CoordinatorRepo.Delete(id);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                Response r = new Response() { Success = false };
                r.ErrorList.Add("Error on delete Coordinator");
                return r;
            }
        }

        public async Task<DataResponse<Coordinator>> GetAll()
        {
            try
            {
                DataResponse<Coordinator> response = await _CoordinatorRepo.GetAll();
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                DataResponse<Coordinator> r = new DataResponse<Coordinator>() { Success = false };
                r.ErrorList.Add("Error on read Coordinators");
                return r;
            }
        }

        public async Task<DataResponse<Coordinator>> GetByID(int id)
        {
            try
            {
                DataResponse<Coordinator> response = await _CoordinatorRepo.GetByID(id);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                DataResponse<Coordinator> r = new DataResponse<Coordinator>() { Success = false };
                r.ErrorList.Add("Error on get Coordinator");
                return r;
            }
        }

        public async Task<DataResponse<Coordinator>> Update(Coordinator item)
        {
            DataResponse<Coordinator> response = new DataResponse<Coordinator>();
            try
            {
                response = await _CoordinatorRepo.Update(item);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.ErrorList.Add("Error on update Coordinator");
                response.Success = false;
                return response;
            }
        }

        public async Task<Response> AddClass(Coordinator coordinator, Class @class)
        {
            Response response = new Response();
            try
            {
                response = await _CoordinatorRepo.AddClass(coordinator, @class);
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Error while addding Class in coordinator");
                return response;
            }
        }
    }
}
