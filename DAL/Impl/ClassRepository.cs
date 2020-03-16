using DAL.Interfaces;
using Entities;
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
    public class ClassRepository : IClassRepository
    {
        private AcademyContext _context;
        public ClassRepository(AcademyContext context)
        {
            _context = context;
        }
        public async Task<Response> Create(Class item)
        {
            Response response = new Response();
            try
            {
                item.Subject = null;
                item.Course = null;

                item.CreatedAt = DateTime.Now;
                await _context.Classes.AddAsync(item);
                await _context.SaveChangesAsync();
                return response;

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

                Class Class = await _context.Classes.FindAsync(id);
                Class.IsActive = false;
                Class.DeletedAt = DateTime.Now;
                _context.Classes.Update(Class);
                await _context.SaveChangesAsync();
                return response;

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

                response.Data = await _context.Classes.Include(c => c.Instructors).Where(a => a.IsActive == true).ToListAsync();
                return response;

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

                response.Data.Add(await _context.Classes
                    .AsNoTracking()
                    .Include(c => c.Attendances)
                    .Include(c => c.Coordinators)
                    .Include(c => c.Course)
                    .Include(c => c.Students)
                    .Include(c => c.Subject)
                    .SingleOrDefaultAsync(c => c.IsActive && c.ID == id)
                );

                return response;
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

                item.UpdatedAt = DateTime.Now;
                _context.Classes.Update(item);
                await _context.SaveChangesAsync();
                return response;

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

                InstructorClass instructorClass = new InstructorClass()
                {
                    ClassID = Class.ID,
                    InstructorID = instructor.ID
                };
                (await _context.Classes.Include(c => c.Instructors).Where(c => c.ID == Class.ID).FirstOrDefaultAsync()).Instructors.Add(instructorClass);
                await _context.SaveChangesAsync();
                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind instructor to class.");
                return response;
            }
        }

        public async Task<Response> AddStudent(Class Class, Student student)
        {
            Response response = new Response();
            try
            {

                StudentClass studentClass = new StudentClass()
                {
                    ClassID = Class.ID,
                    StudentID = student.ID
                };
                (await _context.Classes.Include(c => c.Students).Where(c => c.ID == Class.ID).FirstOrDefaultAsync()).Students.Add(studentClass);
                await _context.SaveChangesAsync();
                return response;
            
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind student to class.");
                return response;
            }
        }

        public async Task<Response> AddEvaluation(Class Class, Evaluation evaluation)
        {
            Response response = new Response();
            try
            {

                (await _context.Classes.Include(c => c.Evaluations).Where(c => c.ID == Class.ID).FirstOrDefaultAsync()).Evaluations.Add(evaluation);
                await _context.SaveChangesAsync();
                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind evaluation to class.");
                return response;
            }
        }

        public async Task<Response> AddAttendance(Class Class, Attendance attendance)
        {
            Response response = new Response();
            try
            {
                (await _context.Classes.Include(c => c.Attendances).Where(c => c.ID == Class.ID).FirstOrDefaultAsync()).Attendances.Add(attendance);
                await _context.SaveChangesAsync();
                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind attendance to class.");
                return response;
            }
        }

        public async Task<Response> AddCoordinator(Class Class, Coordinator coordinator)
        {
            Response response = new Response();
            try
            {

                CoordinatorClass coordinatorClass = new CoordinatorClass()
                {
                    ClassID = Class.ID,
                    CoordinatorID = coordinator.ID
                };
                (await _context.Classes.Include(c => c.Coordinators).Where(c => c.ID == Class.ID).FirstOrDefaultAsync()).Coordinators.Add(coordinatorClass);
                await _context.SaveChangesAsync();
                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind coordinator to class.");
                return response;
            }
        }

        public async Task<DataResponse<int>> CreateAndReturnID(Class item)
        {
            DataResponse<int> response = new DataResponse<int>();
            try
            {
                item.CreatedAt = DateTime.Now;
                
                await _context.Classes.AddAsync(item);
                await _context.SaveChangesAsync();
                await _context.Entry(item).GetDatabaseValuesAsync();
                
                response.Data.Add(item.ID);
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
    }
}
