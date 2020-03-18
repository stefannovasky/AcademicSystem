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
    public class StudentRepository : IStudentRepository
    {
        private AcademyContext _context;
        public StudentRepository(AcademyContext academyContext)
        {
            _context = academyContext;
        }

        public async Task<Response> Create(Student item)
        {
            try
            {
                item.User = null;
                    await _context.Students.AddAsync(item);
                    await _context.SaveChangesAsync();

                return new Response() { Success = true };
            }
            catch (Exception ex)
            {
                Response r = new Response() { Success = false };

                if (ex.InnerException.Message.Contains("unique index"))
                {
                    r.ErrorList.Add("Student already exists");
                } else
                {
                    r.ErrorList.Add("Insert student error");
                }

                return r;
            }
        }

        public async Task<Response> Delete(int id)
        {
            try
            {
                    Student u = await _context.Students.FindAsync(id);
                    u.IsActive = false;
                    u.DeletedAt = DateTime.Now; 
                    _context.Update(u);
                    await _context.SaveChangesAsync();
                return new Response();
            }
            catch (Exception ex)
            {
                Response r = new Response() { Success = false };
                r.ErrorList.Add("Delete student error");
                return r;
            }
        }

        public async Task<DataResponse<Student>> GetAll()
        {
            try
            {
                List<Student> students = new List<Student>();
                    students = await _context.Students
                        .Where(u => u.IsActive == true)
                        .ToListAsync();
                DataResponse<Student> r = new DataResponse<Student>();
                r.Data = students;

                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Student> r = new DataResponse<Student>() { Success = false };
                r.ErrorList.Add("Get all students error");
                return r;
            }
        }

        public async Task<DataResponse<Student>> GetByID(int id)
        {
            try
            {
                Student student = new Student();
                    student = await _context.Students.Include(u => u.User).Include(s => s.Classes).Include(s => s.Evaluations).SingleOrDefaultAsync(u => u.IsActive == true && u.ID == id);
                if (student == null)
                {
                    DataResponse<Student> response = new DataResponse<Student>() { Success = false };
                    response.ErrorList.Add("User not found");
                    return response;
                }

                DataResponse<Student> r = new DataResponse<Student>();
                r.Data.Add(student);

                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Student> r = new DataResponse<Student>() { Success = false };
                r.ErrorList.Add("Get student error");
                return r;
            }
        }

        public async Task<DataResponse<Student>> Update(Student item)
        {
            try
            {
                Student u = new Student();
                    u = await _context.Students.AsNoTracking().SingleOrDefaultAsync(u => u.ID == item.ID);
                    u = item;
                    u.UpdatedAt = DateTime.Now; 
                    _context.Update(u);
                    await _context.SaveChangesAsync();


                DataResponse<Student> r = new DataResponse<Student>();
                r.Data.Add(u);
                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Student> r = new DataResponse<Student>() { Success = false };
                r.ErrorList.Add("Update student error");
                return r;
            }
        }

        public async Task<Response> AddEvaluation(Student student, Evaluation evaluation)
        {
            Response response = new Response();
            try
            {

                (await _context.Students.Include(c => c.Evaluations).Where(c => c.ID == student.ID).FirstOrDefaultAsync()).Evaluations.Add(evaluation);
                await _context.SaveChangesAsync();
                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind evaluation to student.");
                return response;
            }
        }

        public async Task<Response> AddClass(Student student, Class Class)
        {
            Response response = new Response();
            try
            {
                    StudentClass studentClass = new StudentClass()
                    {
                        StudentID = student.ID,
                        ClassID = Class.ID
                    };
                    (await _context.Students.Include(c => c.Classes).Where(c => c.ID == student.ID).FirstOrDefaultAsync()).Classes.Add(studentClass);
                    await _context.SaveChangesAsync();
                    return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind class to student.");
                return response;
            }
        }

        public async Task<Response> AddAttendance(Student student, Attendance attendance)
        {
            Response response = new Response();
            try
            {
                    (await _context.Students.Include(c => c.Attendances).Where(c => c.ID == student.ID).FirstOrDefaultAsync()).Attendances.Add(attendance);
                    await _context.SaveChangesAsync();
                    return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind attendance to student.");
                return response;
            }
        }
    }
}
