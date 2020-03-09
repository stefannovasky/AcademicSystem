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
    public class OwnerRepository : IOwnerRepository
    {
        public async Task<Response> Create(Owner item)
        {
            try
            {
                item.User = null;
                using (AcademyContext ctx = new AcademyContext())
                {
                    await ctx.Owners.AddAsync(item);
                    await ctx.SaveChangesAsync();
                }

                return new Response() { Success = true };
            }
            catch (Exception ex)
            {
                Response r = new Response() { Success = false };

                if (ex.InnerException.Message.Contains("unique index"))
                {
                    r.ErrorList.Add("Owner already exists");
                }
                else
                {
                    r.ErrorList.Add("Insert owner error");
                }

                return r;
            }
        }

        public async Task<Response> Delete(int id)
        {
            try
            {
                using (AcademyContext ctx = new AcademyContext())
                {
                    Owner u = await ctx.Owners.FindAsync(id);
                    u.IsActive = false;
                    u.DeletedAt = DateTime.Now;
                    ctx.Update(u);
                    await ctx.SaveChangesAsync();
                }
                return new Response();
            }
            catch (Exception ex)
            {
                Response r = new Response() { Success = false };
                r.ErrorList.Add("Delete Owner error");
                return r;
            }
        }

        public async Task<DataResponse<Owner>> GetAll()
        {
            try
            {
                List<Owner> owners = new List<Owner>();

                using (AcademyContext ctx = new AcademyContext())
                {
                    owners = await ctx.Owners
                        .Include(u => u.User)
                        .Where(u => u.IsActive == true)
                        .ToListAsync();
                }
                DataResponse<Owner> r = new DataResponse<Owner>();
                r.Data = owners;

                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Owner> r = new DataResponse<Owner>() { Success = false };
                r.ErrorList.Add("Get all owners error");
                return r;
            }
        }

        public async Task<DataResponse<Owner>> GetByID(int id)
        {
            try
            {
                Owner owner = new Owner();

                using (AcademyContext ctx = new AcademyContext())
                {
                    owner = await ctx.Owners.Include(u => u.User).SingleOrDefaultAsync(u => u.IsActive == true && u.ID == id);
                }
                if (owner == null)
                {
                    DataResponse<Owner> response = new DataResponse<Owner>() { Success = false };
                    response.ErrorList.Add("User not found");
                    return response;
                }

                DataResponse<Owner> r = new DataResponse<Owner>();
                r.Data.Add(owner);

                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Owner> r = new DataResponse<Owner>() { Success = false };
                r.ErrorList.Add("Get Owner error");
                return r;
            }
        }

        public async Task<DataResponse<Owner>> Update(Owner item)
        {
            try
            {
                Owner u = new Owner();
                using (AcademyContext ctx = new AcademyContext())
                {
                    u = await ctx.Owners.AsNoTracking().SingleOrDefaultAsync(u => u.ID == item.ID);
                    u = item;
                    u.UpdatedAt = DateTime.Now;
                    ctx.Update(u);
                    await ctx.SaveChangesAsync();
                }

                DataResponse<Owner> r = new DataResponse<Owner>();
                r.Data.Add(u);
                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Owner> r = new DataResponse<Owner>() { Success = false };
                r.ErrorList.Add("Update Owner error");
                return r;
            }
        }
    }
}
