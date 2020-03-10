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

        public async Task<Response> AddStudent(Class Class, Student student)
        {
            Response response = new Response();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    StudentClass studentClass = new StudentClass()
                    {
                        ClassID = Class.ID,
                        StudentID = student.ID
                    };
                    (await context.Classes.Include(c => c.Students).Where(c => c.ID == Class.ID).FirstOrDefaultAsync()).Students.Add(studentClass);
                    await context.SaveChangesAsync();
                    return response;
                }
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
                using (AcademyContext context = new AcademyContext())
                {
                    EvaluationClass evaluationClass = new EvaluationClass()
                    {
                        ClassID = Class.ID,
                        EvaluationID = evaluation.ID
                    };
                    (await context.Classes.Include(c => c.Evaluations).Where(c => c.ID == Class.ID).FirstOrDefaultAsync()).Evaluations.Add(evaluationClass);
                    await context.SaveChangesAsync();
                    return response;
                }
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
                using (AcademyContext context = new AcademyContext())
                {
                    AttendanceClass attendanceClass = new AttendanceClass()
                    {
                        ClassID = Class.ID,
                        AttendanceID = attendance.ID
                    };
                    (await context.Classes.Include(c => c.Attendances).Where(c => c.ID == Class.ID).FirstOrDefaultAsync()).Attendances.Add(attendanceClass);
                    await context.SaveChangesAsync();
                    return response;
                }
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
                using (AcademyContext context = new AcademyContext())
                {
                    CoordinatorClass coordinatorClass = new CoordinatorClass()
                    {
                        ClassID = Class.ID,
                        CoordinatorID = coordinator.ID
                    };
                    (await context.Classes.Include(c => c.Coordinators).Where(c => c.ID == Class.ID).FirstOrDefaultAsync()).Coordinators.Add(coordinatorClass);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while addind coordinator to class.");
                return response;
            }
        }
    }
}
