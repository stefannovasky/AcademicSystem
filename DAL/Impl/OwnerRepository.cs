using DAL.Interfaces;
using Entities;
using log4net;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Impl
{
    public class OwnerRepository : IOwnerRepository
    {
        private AcademyContext _context;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public OwnerRepository(AcademyContext academyContext)
        {
            _context = academyContext;
        }
        public async Task<Response> Create(Owner item)
        {
            try
            {
                item.User = null;
                    await _context.Owners.AddAsync(item);
                    await _context.SaveChangesAsync();
                return new Response() { Success = true };
            }
            catch (Exception e)
            {
                Response r = new Response() { Success = false };

                if (e.InnerException.Message.Contains("unique index"))
                {
                    r.ErrorList.Add("Owner already exists");
                }
                else
                {
                    r.ErrorList.Add("Insert owner error");
                }
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                return r;
            }
        }

        public async Task<Response> Delete(int id)
        {
            try
            {
                    Owner u = await _context.Owners.FindAsync(id);
                    u.IsActive = false;
                    u.DeletedAt = DateTime.Now;
                    _context.Update(u);
                    await _context.SaveChangesAsync();
                return new Response();
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
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

                    owners = await _context.Owners
                        .Include(u => u.User)
                        .Where(u => u.IsActive == true)
                        .ToListAsync();
                DataResponse<Owner> r = new DataResponse<Owner>();
                r.Data = owners;

                return r;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
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

                    owner = await _context.Owners.Include(u => u.User).Include(u => u.Courses).SingleOrDefaultAsync(u => u.IsActive == true && u.ID == id);
                if (owner == null)
                {
                    DataResponse<Owner> response = new DataResponse<Owner>() { Success = false };
                    response.ErrorList.Add("Owner not found");
                    return response;
                }

                DataResponse<Owner> r = new DataResponse<Owner>();
                r.Data.Add(owner);

                return r;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
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
                    u = await _context.Owners.AsNoTracking().SingleOrDefaultAsync(u => u.ID == item.ID);
                    u = item;
                    u.UpdatedAt = DateTime.Now;
                    _context.Update(u);
                    await _context.SaveChangesAsync();

                DataResponse<Owner> r = new DataResponse<Owner>();
                r.Data.Add(u);
                return r;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                DataResponse<Owner> r = new DataResponse<Owner>() { Success = false };
                r.ErrorList.Add("Update Owner error");
                return r;
            }
        }

        public async Task<Response> AddCourse(Owner owner, Course course)
        {
            Response response = new Response();
            try
            {
                _context.Entry(owner).State = EntityState.Detached;
                _context.Entry(course).State = EntityState.Detached;

                OwnerCourse ownerCourse = new OwnerCourse()
                {
                    OwnerID = owner.ID,
                    CourseID = course.ID
                };
                (await _context.Owners.Include(c => c.Courses).Where(c => c.ID == owner.ID).FirstOrDefaultAsync()).Courses.Add(ownerCourse);
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Error while addind course to owner.");
                return response;
            }
        }
    }
}
