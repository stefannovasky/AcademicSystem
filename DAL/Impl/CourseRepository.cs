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
        public async Task<Response> Create(Course item)
        {
            Response response = new Response();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    item.CreatedAt = DateTime.Now;
                    await context.Courses.AddAsync(item);
                    await context.SaveChangesAsync();
                    return response;
                }
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
                using (AcademyContext context = new AcademyContext())
                {
                    Course Course = await context.Courses.FindAsync(id);
                    Course.IsActive = false;
                    Course.DeletedAt = DateTime.Now;
                    context.Courses.Update(Course);
                    await context.SaveChangesAsync();
                    return response;
                }
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
                using (AcademyContext context = new AcademyContext())
                {
                    response.Data = await context.Courses.Where(a => a.IsActive == true).ToListAsync();
                    return response;
                }
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
                using (AcademyContext context = new AcademyContext())
                {
                    response.Data.Add(await context.Courses.FindAsync(id));
                    return response;
                }
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
                using (AcademyContext context = new AcademyContext())
                {
                    item.UpdatedAt = DateTime.Now;
                    context.Courses.Update(item);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while updating Course.");
                return response;
            }
        }
    }
}
