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
    public class SubjectRepository : ISubjectRepository
    {
        public async Task<Response> Create(Subject item)
        {
            try
            {
                item.Course = null; 
                using (AcademyContext ctx = new AcademyContext())
                {
                    await ctx.Subjects.AddAsync(item);
                    await ctx.SaveChangesAsync();
                }

                return new Response() { Success = true };
            }
            catch (Exception ex)
            {
                Response r = new Response() { Success = false };

                if (ex.InnerException.Message.Contains("unique index"))
                {
                    r.ErrorList.Add("Subject already exists");
                }
                else
                {
                    r.ErrorList.Add("Insert Subject error");
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
                    Subject u = await ctx.Subjects.FindAsync(id);
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
                r.ErrorList.Add("Delete Subject error");
                return r;
            }
        }

        public async Task<DataResponse<Subject>> GetAll()
        {
            try
            {
                List<Subject> Subjects = new List<Subject>();

                using (AcademyContext ctx = new AcademyContext())
                {
                    Subjects = await ctx.Subjects
                        .Where(u => u.IsActive == true)
                        .ToListAsync();
                }
                DataResponse<Subject> r = new DataResponse<Subject>();
                r.Data = Subjects;

                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Subject> r = new DataResponse<Subject>() { Success = false };
                r.ErrorList.Add("Get all Subjects error");
                return r;
            }
        }

        public async Task<DataResponse<Subject>> GetByID(int id)
        {
            try
            {
                Subject Subject = new Subject();

                using (AcademyContext ctx = new AcademyContext())
                {
                    Subject = await ctx.Subjects.SingleOrDefaultAsync(u => u.IsActive == true && u.ID == id);
                }
                if (Subject == null)
                {
                    DataResponse<Subject> response = new DataResponse<Subject>() { Success = false };
                    response.ErrorList.Add("User not found");
                    return response;
                }

                DataResponse<Subject> r = new DataResponse<Subject>();
                r.Data.Add(Subject);

                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Subject> r = new DataResponse<Subject>() { Success = false };
                r.ErrorList.Add("Get Subject error");
                return r;
            }
        }

        public async Task<DataResponse<Subject>> Update(Subject item)
        {
            try
            {
                Subject u = new Subject();
                using (AcademyContext ctx = new AcademyContext())
                {
                    u = await ctx.Subjects.AsNoTracking().SingleOrDefaultAsync(u => u.ID == item.ID);
                    u = item;
                    u.UpdatedAt = DateTime.Now;
                    ctx.Update(u);
                    await ctx.SaveChangesAsync();
                }

                DataResponse<Subject> r = new DataResponse<Subject>();
                r.Data.Add(u);
                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Subject> r = new DataResponse<Subject>() { Success = false };
                r.ErrorList.Add("Update Subject error");
                return r;
            }
        }

        public async Task<Response> AddInstructor(Subject subject, Instructor instructor)
        {
            Response response = new Response();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    SubjectInstructor subjectInstructor = new SubjectInstructor()
                    {
                        SubjectID = subject.ID,
                        InstructorID = instructor.ID
                    };
                    (await context.Subjects.Include(c => c.Instructors).Where(c => c.ID == subject.ID).FirstOrDefaultAsync()).Instructors.Add(subjectInstructor);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind instructor to subject.");
                return response;
            }
        }

        public async Task<Response> AddClass(Subject subject, Class Class)
        {
            Response response = new Response();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    (await context.Subjects.Include(c => c.Classes).Where(c => c.ID == subject.ID).FirstOrDefaultAsync()).Classes.Add(Class);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind class to subject.");
                return response;
            }
        }
    }
}
