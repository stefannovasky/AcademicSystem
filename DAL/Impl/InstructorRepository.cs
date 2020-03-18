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
    public class InstructorRepository : IInstructorRepository
    {
        private AcademyContext _context;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public InstructorRepository(AcademyContext academyContext)
        {
            _context = academyContext;
        }

        public async Task<Response> Create(Instructor item)
        {
            try
            {
                item.User = null;
                    await _context.Instructors.AddAsync(item);
                    await _context.SaveChangesAsync();

                return new Response() { Success = true };
            }
            catch (Exception e)
            {
                Response r = new Response() { Success = false };

                if (e.InnerException.Message.Contains("unique index"))
                {
                    r.ErrorList.Add("Instructor already exists");
                }
                else
                {
                    r.ErrorList.Add("Insert Instructor error");
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
                    Instructor u = await _context.Instructors.FindAsync(id);
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
                r.ErrorList.Add("Delete Instructor error");
                return r;
            }
        }

        public async Task<DataResponse<Instructor>> GetAll()
        {
            try
            {
                List<Instructor> Instructors = new List<Instructor>();

                    Instructors = await _context.Instructors
                        .Where(u => u.IsActive == true)
                        .ToListAsync();
                DataResponse<Instructor> r = new DataResponse<Instructor>();
                r.Data = Instructors;

                return r;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
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

                    Instructor = await _context.Instructors.Include(u => u.User).Include(u => u.Classes).Include(u => u.Subjects).SingleOrDefaultAsync(u => u.IsActive == true && u.ID == id);
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
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
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
                    u = await _context.Instructors.AsNoTracking().SingleOrDefaultAsync(u => u.ID == item.ID);
                    u = item;
                    u.UpdatedAt = DateTime.Now;
                    _context.Update(u);
                    await _context.SaveChangesAsync();

                DataResponse<Instructor> r = new DataResponse<Instructor>();
                r.Data.Add(u);
                return r;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                DataResponse<Instructor> r = new DataResponse<Instructor>() { Success = false };
                r.ErrorList.Add("Update Instructor error");
                return r;
            }
        }

        public async Task<Response> AddSubject(Instructor instructor, Subject subject)
        {
            Response response = new Response();
            try
            {
                    SubjectInstructor subjectInstructor = new SubjectInstructor()
                    {
                        SubjectID = subject.ID,
                        InstructorID = instructor.ID
                    };
                    (await _context.Instructors.Include(c => c.Subjects).Where(c => c.ID == instructor.ID).FirstOrDefaultAsync()).Subjects.Add(subjectInstructor);
                    await _context.SaveChangesAsync();
                    return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Error while addind subject to instructor.");
                return response;
            }
        }

        public async Task<Response> AddClass(Instructor instructor, Class Class)
        {
            Response response = new Response();
            try
            {
                    InstructorClass instructorClass = new InstructorClass()
                    {
                        ClassID = Class.ID,
                        InstructorID = instructor.ID
                    };
                    (await _context.Instructors.Include(c => c.Classes).Where(c => c.ID == instructor.ID).FirstOrDefaultAsync()).Classes.Add(instructorClass);
                    await _context.SaveChangesAsync();
                    return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Error while addind class to instructor.");
                return response;
            }
        }
    }
}
