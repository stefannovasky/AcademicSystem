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
        public Task<Response> Create(Attendance item)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Attendance>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Attendance>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataResponse<Attendance>> Update(Attendance item)
        {
            throw new NotImplementedException();
        }
    }
}
