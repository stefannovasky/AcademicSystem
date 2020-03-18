using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<DataResponse<User>> GetByEmail(string email);
        Task<Response> AddOwner(User user, Owner owner);
        Task<Response> AddStudent(User user, Student student);
        Task<Response> AddCoordinator(User user, Coordinator coordinator);
        Task<Response> AddInstructor(User user, Instructor instructor);
    }
}
