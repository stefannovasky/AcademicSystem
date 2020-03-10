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
    public class ClassRepository : IClassRepository
    {
        public async Task<Response> Create(Class item)
        {
            Response response = new Response();
            try
            {
                item.Subject = null;
                item.Course = null;
                using (AcademyContext context = new AcademyContext())
                {
                    item.CreatedAt = DateTime.Now;
                    await context.Classes.AddAsync(item);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while adding Class.");
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
                    Class Class = await context.Classes.FindAsync(id);
                    Class.IsActive = false;
                    Class.DeletedAt = DateTime.Now;
                    context.Classes.Update(Class);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Invalid Class Id");
                return response;
            }
        }

        public async Task<DataResponse<Class>> GetAll()
        {
            DataResponse<Class> response = new DataResponse<Class>();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    response.Data = await context.Classes.Include(c => c.Instructors).Where(a => a.IsActive == true).ToListAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while getting Classs.");
                return response;
            }
        }

        public async Task<DataResponse<Class>> GetByID(int id)
        {
            DataResponse<Class> response = new DataResponse<Class>();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    response.Data.Add(await context.Classes.FindAsync(id));
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while getting Class.");
                return response;
            }
        }

        public async Task<DataResponse<Class>> Update(Class item)
        {
            DataResponse<Class> response = new DataResponse<Class>();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    item.UpdatedAt = DateTime.Now;
                    context.Classes.Update(item);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while updating Class.");
                return response;
            }
        }

        public async Task<Response> AddInstructor(Class Class, Instructor instructor)
        {
            Response response = new Response();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    InstructorClass instructorClass = new InstructorClass() 
                    {
                        ClassID = Class.ID,
                        InstructorID = instructor.ID
                    };
                    (await context.Classes.Include(c => c.Instructors).Where(c => c.ID == Class.ID).FirstOrDefaultAsync()).Instructors.Add(instructorClass);
                    //Class.Instructors.Add(instructorClass);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind instructor to class.");
                return response;
            }
        }
    }
}
