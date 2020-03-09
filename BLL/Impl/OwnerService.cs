using BLL.Interfaces;
using DAL.Impl;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    public class OwnerService : IOwnerService
    {
        private OwnerRepository _OwnerRepo = new OwnerRepository();
        public async Task<Response> Create(Owner item)
        {
            Response response = new Response();
            try
            {
                response = await _OwnerRepo.Create(item);
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorList.Add("Error on create Owner");
                response.Success = false;
                return response;
            }
        }

        public async Task<Response> Delete(int id)
        {
            try
            {
                Response response = await _OwnerRepo.Delete(id);
                return response;
            }
            catch (Exception)
            {
                Response r = new Response() { Success = false };
                r.ErrorList.Add("Error on delete Owner");
                return r;
            }
        }

        public async Task<DataResponse<Owner>> GetAll()
        {
            try
            {
                DataResponse<Owner> response = await _OwnerRepo.GetAll();
                return response;
            }
            catch (Exception)
            {
                DataResponse<Owner> r = new DataResponse<Owner>() { Success = false };
                r.ErrorList.Add("Error on read Owners");
                return r;
            }
        }

        public async Task<DataResponse<Owner>> GetByID(int id)
        {
            try
            {
                DataResponse<Owner> response = await _OwnerRepo.GetByID(id);
                return response;
            }
            catch (Exception)
            {
                DataResponse<Owner> r = new DataResponse<Owner>() { Success = false };
                r.ErrorList.Add("Error on get Owner");
                return r;
            }
        }

        public async Task<DataResponse<Owner>> Update(Owner item)
        {
            DataResponse<Owner> response = new DataResponse<Owner>();
            try
            {
                response = await _OwnerRepo.Update(item);
                return response;
            }
            catch (Exception)
            {
                response.ErrorList.Add("Error on update Owner");
                response.Success = false;
                return response;
            }
        }
    }
}
