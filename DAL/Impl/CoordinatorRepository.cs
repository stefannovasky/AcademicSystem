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
        private AcademyContext _context;
        public CoordinatorRepository(AcademyContext context)
        {
            _context = context;
        }
        public async Task<Response> Create(Coordinator item)
        {
            Response response = new Response();
            try
            {
                item.User = null;

                item.CreatedAt = DateTime.Now;
                await _context.Coordinators.AddAsync(item);
                await _context.SaveChangesAsync();
                return response;

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

                Coordinator Coordinator = await _context.Coordinators.FindAsync(id);
                Coordinator.IsActive = false;
                Coordinator.DeletedAt = DateTime.Now;
                _context.Coordinators.Update(Coordinator);
                await _context.SaveChangesAsync();
                return response;
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

                response.Data = await _context.Coordinators.Where(a => a.IsActive == true).ToListAsync();
                return response;
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
                Coordinator c = new Coordinator();
                c = await _context.Coordinators
                    .Include(c => c.User)
                    .Include(c => c.Classes)
                    .SingleOrDefaultAsync(c => c.IsActive && c.ID == id);
                if (c == null)
                {
                    response.Success = false;
                    response.ErrorList.Add("User not found");
                    return response;
                }
                response.Data.Add(c);
                return response;

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
                item.UpdatedAt = DateTime.Now;
                _context.Coordinators.Update(item);
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while updating Coordinator.");
                return response;
            }
        }

        public async Task<Response> AddClass(Coordinator coordinator, Class Class)
        {
            Response response = new Response();
            try
            {

                CoordinatorClass coordinatorClass = new CoordinatorClass()
                {
                    ClassID = Class.ID,
                    CoordinatorID = coordinator.ID
                };
                (await _context.Coordinators.Include(c => c.Classes).Where(c => c.ID == coordinator.ID).FirstOrDefaultAsync()).Classes.Add(coordinatorClass);
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind class to coordinator.");
                return response;
            }
        }
    }
}
