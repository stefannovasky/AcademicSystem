using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IClassRepository : IRepository<Class>
    {
        public Task<Response> AddInstructor(Class Class, Instructor instructor);


        public Task<Response> AddStudent(Class Class, Student student);


        public Task<Response> AddEvaluation(Class Class, Evaluation evaluation);
 

        public  Task<Response> AddAttendance(Class Class, Attendance attendance);
 

        public Task<Response> AddCoordinator(Class Class, Coordinator coordinator);
        Task<DataResponse<int>> CreateAndReturnID(Class item);

    }
}
