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
    public class SubjectRepository : ISubjectRepository
    {
        private AcademyContext _context;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SubjectRepository(AcademyContext academyContext)
        {
            _context = academyContext;
        }

        public async Task<Response> Create(Subject item)
        {
            try
            {
                item.Course = null; 
                    await _context.Subjects.AddAsync(item);
                    await _context.SaveChangesAsync();

                return new Response() { Success = true };
            }
            catch (Exception e)
            {
                Response r = new Response() { Success = false };

                if (e.InnerException.Message.Contains("unique index"))
                {
                    r.ErrorList.Add("Subject already exists");
                }
                else
                {
                    r.ErrorList.Add("Insert Subject error");
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
                Subject u = await _context.Subjects.FindAsync(id);
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
                r.ErrorList.Add("Delete Subject error");
                return r;
            }
        }

        public async Task<DataResponse<Subject>> GetAll()
        {
            try
            {
                List<Subject> Subjects = new List<Subject>();

                    Subjects = await _context.Subjects
                        .Where(u => u.IsActive == true)
                        .ToListAsync();
                DataResponse<Subject> r = new DataResponse<Subject>();
                r.Data = Subjects;

                return r;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
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

                    Subject = await _context.Subjects.Include(s => s.Classes).Include(s => s.Course).Include(s => s.Instructors).SingleOrDefaultAsync(u => u.IsActive == true && u.ID == id);
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
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
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
                    u = await _context.Subjects.AsNoTracking().SingleOrDefaultAsync(u => u.ID == item.ID);
                    u = item;
                    u.UpdatedAt = DateTime.Now;
                    _context.Update(u);
                    await _context.SaveChangesAsync();

                DataResponse<Subject> r = new DataResponse<Subject>();
                r.Data.Add(u);
                return r;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
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
                    SubjectInstructor subjectInstructor = new SubjectInstructor()
                    {
                        SubjectID = subject.ID,
                        InstructorID = instructor.ID
                    };
                    (await _context.Subjects.Include(c => c.Instructors).Where(c => c.ID == subject.ID).FirstOrDefaultAsync()).Instructors.Add(subjectInstructor);
                    await _context.SaveChangesAsync();
                    return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
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
                    (await _context.Subjects.Include(c => c.Classes).Where(c => c.ID == subject.ID).FirstOrDefaultAsync()).Classes.Add(Class);
                    await _context.SaveChangesAsync();
                    return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Error while addind class to subject.");
                return response;
            }
        }
    }
}
