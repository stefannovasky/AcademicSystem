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
        private AcademyContext _context;
        public UserRepository(AcademyContext context)
        {
            _context = context;
        }

        public async Task<Response> AddCoordinator(User user, Coordinator coordinator)
        {
            Response r = new Response();
            try
            {
                    (await _context.Users.Include(c => c.Coordinator).Where(c => c.ID == user.ID).FirstOrDefaultAsync()).Coordinator = coordinator;
                    await _context.SaveChangesAsync();
                return r; 
            }
            catch (Exception)
            {
                r.Success = false;
                r.ErrorList.Add("Error on add coordinator to user");
                return r; 
            }
        }

        public async Task<Response> AddInstructor(User user, Instructor instructor)
        {
            Response r = new Response();
            try
            {
                    (await _context.Users.Include(c => c.Instructor).Where(c => c.ID == user.ID).FirstOrDefaultAsync()).Instructor = instructor;
                    await _context.SaveChangesAsync();
                return r;
            }
            catch (Exception)
            {
                r.Success = false;
                r.ErrorList.Add("Error on add instructor to user");
                return r;
            }
        }

        public async Task<Response> AddOwner(User user, Owner owner)
        {
            Response r = new Response();
            try
            {
                    (await _context.Users.Include(c => c.Owner).Where(c => c.ID == user.ID).FirstOrDefaultAsync()).Owner = owner;
                    await _context.SaveChangesAsync();
                return r;
            }
            catch (Exception)
            {
                r.Success = false;
                r.ErrorList.Add("Error on add owner to user");
                return r;
            }
        }

        public async Task<Response> AddStudent(User user, Student student)
        {
            Response r = new Response();
            try
            {
                    (await _context.Users.Include(c => c.Student).Where(c => c.ID == user.ID).FirstOrDefaultAsync()).Student = student;
                    await _context.SaveChangesAsync();
                return r;
            }
            catch (Exception)
            {
                r.Success = false;
                r.ErrorList.Add("Error on add owner to user");
                return r;
            }
        }

        public async Task<Response> Create(User item)
        {
            try
            {
                    await _context.Users.AddAsync(item);
                    await _context.SaveChangesAsync();
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
                    User u = await _context.Users.FindAsync(id);
                    u.IsActive = false;
                    u.DeletedAt = DateTime.Now;
                    _context.Update(u);
                    await _context.SaveChangesAsync();
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
                    users = await _context.Users
                        .Where(u => u.IsActive == true)
                        .ToListAsync();
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
                    user = await _context.Users
                        .Include(u => u.Student)
                        .Include(u => u.Instructor)
                        .Include(u => u.Coordinator)
                        .Include(u => u.Owner)
                        .SingleOrDefaultAsync(u => u.IsActive == true && u.Email == email);
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

                    user = await _context.Users
                        .Include(u => u.Student)
                        .Include(u => u.Coordinator)
                        .Include(u => u.Owner)
                        .Include(u => u.Instructor)
                        .SingleOrDefaultAsync(u => u.IsActive == true && u.ID == id);
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
                    u = await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.ID == item.ID);
                    u.UpdatedAt = DateTime.Now;
                    u = item;
                    _context.Update(u);
                    await _context.SaveChangesAsync();
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
