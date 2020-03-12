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
    public class CourseRepository : ICourseRepository
    {
        private AcademyContext _context;
        public CourseRepository(AcademyContext context)
        {
            _context = context;
        }
        public async Task<Response> Create(Course item)
        {
            Response response = new Response();
            try
            {

                item.CreatedAt = DateTime.Now;
                await _context.Courses.AddAsync(item);
                await _context.SaveChangesAsync();
                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                if (e.Message.Contains("Name"))
                {
                    response.ErrorList.Add("Name is required.");
                }
                if (e.Message.Contains("Period"))
                {
                    response.ErrorList.Add("Period is required.");
                }
                else
                {
                    response.ErrorList.Add("Error while adding Course.");
                }
                return response;
            }
        }

        public async Task<Response> Delete(int id)
        {
            Response response = new Response();
            try
            {

                Course Course = await _context.Courses.FindAsync(id);
                Course.IsActive = false;
                Course.DeletedAt = DateTime.Now;
                _context.Courses.Update(Course);
                await _context.SaveChangesAsync();
                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Invalid Course Id");
                return response;
            }
        }

        public async Task<DataResponse<Course>> GetAll()
        {
            DataResponse<Course> response = new DataResponse<Course>();
            try
            {

                response.Data = await _context.Courses.Where(a => a.IsActive == true).ToListAsync();
                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while getting Courses.");
                return response;
            }
        }

        public async Task<DataResponse<Course>> GetByID(int id)
        {
            DataResponse<Course> response = new DataResponse<Course>();
            try
            {

                response.Data.Add(await _context.Courses.Include(c => c.Classes).Include(c => c.Owners).Include(c => c.Subjects).SingleOrDefaultAsync(c => c.IsActive && c.ID == id));
                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while getting Course.");
                return response;
            }
        }

        public async Task<DataResponse<Course>> Update(Course item)
        {
            DataResponse<Course> response = new DataResponse<Course>();
            try
            {

                item.UpdatedAt = DateTime.Now;
                _context.Courses.Update(item);
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while updating Course.");
                return response;
            }
        }
        // class subject owner 
        public async Task<Response> AddClass(Course course, Class Class)
        {
            Response response = new Response();
            try
            {

                (await _context.Courses.Include(c => c.Classes).Where(c => c.ID == course.ID).FirstOrDefaultAsync()).Classes.Add(Class);
                await _context.SaveChangesAsync();
                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind class to course.");
                return response;
            }
        }

        public async Task<Response> AddSubject(Course course, Subject subject)
        {
            Response response = new Response();
            try
            {

                (await _context.Courses.Include(c => c.Subjects).Where(c => c.ID == course.ID).FirstOrDefaultAsync()).Subjects.Add(subject);
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind subject to course.");
                return response;
            }
        }

        public async Task<Response> AddOwner(Course course, Owner owner)
        {
            Response response = new Response();
            try
            {

                OwnerCourse ownerCourse = new OwnerCourse()
                {
                    OwnerID = owner.ID,
                    CourseID = course.ID
                };
                (await _context.Courses.Include(c => c.Owners).Where(c => c.ID == course.ID).FirstOrDefaultAsync()).Owners.Add(ownerCourse);
                await _context.SaveChangesAsync();
                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind owner to course.");
                return response;
            }
        }
    }
}
