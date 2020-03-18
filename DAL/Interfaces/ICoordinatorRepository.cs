using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICoordinatorRepository : IRepository<Coordinator>
    {
        Task<Response> AddClass(Coordinator coordinator, Class Class);

    }
}
