using DAL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Impl
{
    public class AttendanceRepository : IAttendanceRepository
    {
        Task<Response> IRepository<Attendance>.Create(Attendance item)
        {
            throw new NotImplementedException();
        }

        Task<Response> IRepository<Attendance>.Delete(int id)
        {
            throw new NotImplementedException();
        }

        Task<DataResponse<Attendance>> IRepository<Attendance>.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<DataResponse<Attendance>> IRepository<Attendance>.GetByID(int id)
        {
            throw new NotImplementedException();
        }

        Task<DataResponse<Attendance>> IRepository<Attendance>.Update(Attendance item)
        {
            throw new NotImplementedException();
        }
    }
}
