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
    public class UserRepository : IUserRepository
    {
        public async Task<Response> Create(User item)
        {
            try
            {
                using (AcademyContext ctx = new AcademyContext())
                {
                    await ctx.Users.AddAsync(item);
                    await ctx.SaveChangesAsync();
                }

                return new Response(); 
            }
            catch (Exception ex)
            {
                Response r = new Response() { Success = false };

                if (ex.InnerException.Message.Contains("unique index"))
                {
                    if (ex.InnerException.Message.Contains("Cpf"))
                    {
                        r.ErrorList.Add("Cpf already exists"); 
                    }
                    else if (ex.InnerException.Message.Contains("Rg"))
                    {
                        r.ErrorList.Add("Rg already exists");
                    }
                    else if (ex.InnerException.Message.Contains("Email"))
                    {
                        r.ErrorList.Add("Email already exists");
                    }
                    else
                    {
                        r.ErrorList.Add("Create user error");
                    }
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
                    User u = await ctx.Users.FindAsync(id);
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
                r.ErrorList.Add("Delete user error");
                return r;
            }
        }

        public async Task<DataResponse<User>> GetAll()
        {
            try
            {
                List<User> users = new List<User>();

                using (AcademyContext ctx = new AcademyContext())
                {
                    users = await ctx.Users
                        .Where(u => u.IsActive == true)
                        .ToListAsync();
                }
                DataResponse<User> r = new DataResponse<User>();
                r.Data = users;

                return r;
            }
            catch (Exception ex)
            {
                DataResponse<User> r = new DataResponse<User>() { Success = false };
                r.ErrorList.Add("Get all users error");
                return r; 
            }

        }

        public async Task<DataResponse<User>> GetByEmail(string email)
        {
            try
            {
                User user = new User();

                using (AcademyContext ctx = new AcademyContext())
                {
                    user = await ctx.Users
                        .Include(u => u.Student)
                        .Include(u => u.Instructor)
                        .Include(u => u.Coordinator)
                        .Include(u => u.Owner)
                        .SingleOrDefaultAsync(u => u.IsActive == true && u.Email == email);
                }
                if (user == null)
                {
                    DataResponse<User> response = new DataResponse<User>() { Success = false };
                    response.ErrorList.Add("User not found");
                    return response;
                }

                DataResponse<User> r = new DataResponse<User>() { Success = true };
                r.Data.Add(user);

                return r;
            }
            catch (Exception ex)
            {
                DataResponse<User> r = new DataResponse<User>() { Success = false };
                r.ErrorList.Add("Get user error");
                return r;
            }
        }

        public async Task<DataResponse<User>> GetByID(int id)
        {
            try
            {
                User user = new User();

                using (AcademyContext ctx = new AcademyContext())
                {
                    user = await ctx.Users.Include(u => u.Student).SingleOrDefaultAsync(u => u.IsActive == true && u.ID == id);
                }
                if (user == null)
                {
                    DataResponse<User> response = new DataResponse<User>() { Success = false };
                    response.ErrorList.Add("User not found");
                    return response; 
                }

                DataResponse<User> r = new DataResponse<User>() { Success = true };
                r.Data.Add(user);

                return r;
            }
            catch (Exception ex)
            {
                DataResponse<User> r = new DataResponse<User>() { Success = false };
                r.ErrorList.Add("Get user error");
                return r;
            }
        }

        public async Task<DataResponse<User>> Update(User item)
        {
            try
            {
                User u = new User();
                using (AcademyContext ctx = new AcademyContext())
                {
                    u = await ctx.Users.AsNoTracking().SingleOrDefaultAsync(u => u.ID == item.ID);
                    u.UpdatedAt = DateTime.Now;
                    u = item;
                    ctx.Update(u);
                    await ctx.SaveChangesAsync();
                }

                DataResponse<User> r = new DataResponse<User>();
                r.Data.Add(u);
                return r; 
            }
            catch (Exception ex)
            {
                DataResponse<User> r = new DataResponse<User>() { Success = false };
                r.ErrorList.Add("Update user error");
                return r;
            }
        }
    }
}
