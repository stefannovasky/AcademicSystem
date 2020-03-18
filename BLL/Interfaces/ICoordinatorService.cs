using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICoordinatorService : IService<Coordinator>
    {
        public Task<Response> AddClass(Coordinator coordinator, Class @class);
    }
}
