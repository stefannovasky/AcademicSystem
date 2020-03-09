using DAL.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Impl
{
    public class CoordinatorRepository : ICoordinatorRepository
    {
        public async Task<Response> Create(Coordinator item)
        {
            Response response = new Response();
            try
            {
                item.User = null;
                using (AcademyContext context = new AcademyContext())
                {
                    item.CreatedAt = DateTime.Now;
                    await context.Coordinators.AddAsync(item);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while adding Coordinator.");
                return response;
            }
        }

        public async Task<Response> Delete(int id)
        {
            Response response = new Response();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    Coordinator Coordinator = await context.Coordinators.FindAsync(id);
                    Coordinator.IsActive = false;
                    Coordinator.DeletedAt = DateTime.Now;
                    context.Coordinators.Update(Coordinator);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Invalid Coordinator Id");
                return response;
            }
        }

        public async Task<DataResponse<Coordinator>> GetAll()
        {
            DataResponse<Coordinator> response = new DataResponse<Coordinator>();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    response.Data = await context.Coordinators.Where(a => a.IsActive == true).ToListAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while getting Coordinators.");
                return response;
            }
        }

        public async Task<DataResponse<Coordinator>> GetByID(int id)
        {
            DataResponse<Coordinator> response = new DataResponse<Coordinator>();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    response.Data.Add(await context.Coordinators.FindAsync(id));
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while getting Coordinator.");
                return response;
            }
        }

        public async Task<DataResponse<Coordinator>> Update(Coordinator item)
        {
            DataResponse<Coordinator> response = new DataResponse<Coordinator>();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    item.UpdatedAt = DateTime.Now;
                    context.Coordinators.Update(item);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while updating Coordinator.");
                return response;
            }
        }
    }
}
