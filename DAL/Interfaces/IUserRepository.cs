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
    }
}
