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
    public class InstructorRepository : IInstructorRepository
    {
        public async Task<Response> Create(Instructor item)
        {
            try
            {
                item.User = null;
                using (AcademyContext ctx = new AcademyContext())
                {
                    await ctx.Instructors.AddAsync(item);
                    await ctx.SaveChangesAsync();
                }

                return new Response() { Success = true };
            }
            catch (Exception ex)
            {
                Response r = new Response() { Success = false };

                if (ex.InnerException.Message.Contains("unique index"))
                {
                    r.ErrorList.Add("Instructor already exists");
                }
                else
                {
                    r.ErrorList.Add("Insert Instructor error");
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
                    Instructor u = await ctx.Instructors.FindAsync(id);
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
                r.ErrorList.Add("Delete Instructor error");
                return r;
            }
        }

        public async Task<DataResponse<Instructor>> GetAll()
        {
            try
            {
                List<Instructor> Instructors = new List<Instructor>();

                using (AcademyContext ctx = new AcademyContext())
                {
                    Instructors = await ctx.Instructors
                        .Where(u => u.IsActive == true)
                        .ToListAsync();
                }
                DataResponse<Instructor> r = new DataResponse<Instructor>();
                r.Data = Instructors;

                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Instructor> r = new DataResponse<Instructor>() { Success = false };
                r.ErrorList.Add("Get all Instructors error");
                return r;
            }
        }

        public async Task<DataResponse<Instructor>> GetByID(int id)
        {
            try
            {
                Instructor Instructor = new Instructor();

                using (AcademyContext ctx = new AcademyContext())
                {
                    Instructor = await ctx.Instructors.Include(u => u.User).SingleOrDefaultAsync(u => u.IsActive == true && u.ID == id);
                }
                if (Instructor == null)
                {
                    DataResponse<Instructor> response = new DataResponse<Instructor>() { Success = false };
                    response.ErrorList.Add("User not found");
                    return response;
                }

                DataResponse<Instructor> r = new DataResponse<Instructor>();
                r.Data.Add(Instructor);

                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Instructor> r = new DataResponse<Instructor>() { Success = false };
                r.ErrorList.Add("Get Instructor error");
                return r;
            }
        }

        public async Task<DataResponse<Instructor>> Update(Instructor item)
        {
            try
            {
                Instructor u = new Instructor();
                using (AcademyContext ctx = new AcademyContext())
                {
                    u = await ctx.Instructors.AsNoTracking().SingleOrDefaultAsync(u => u.ID == item.ID);
                    u = item;
                    u.UpdatedAt = DateTime.Now;
                    ctx.Update(u);
                    await ctx.SaveChangesAsync();
                }

                DataResponse<Instructor> r = new DataResponse<Instructor>();
                r.Data.Add(u);
                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Instructor> r = new DataResponse<Instructor>() { Success = false };
                r.ErrorList.Add("Update Instructor error");
                return r;
            }
        }
    }
}
