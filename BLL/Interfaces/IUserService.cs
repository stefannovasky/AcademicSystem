using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService : IService<User>
    {
        Task<DataResponse<User>> Authenticate(User user);
    }
}
