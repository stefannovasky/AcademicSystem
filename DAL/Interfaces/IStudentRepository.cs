using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Response> AddEvaluation(Student student, Evaluation evaluation);
        Task<Response> AddClass(Student student, Class Class);
        Task<Response> AddAttendance(Student student, Attendance attendance);
    }
}
