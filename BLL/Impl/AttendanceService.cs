using BLL.Interfaces;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Impl
{
    class AttendanceService : IAttendanceService
    {
        public async Task<Response> Create(Attendance item)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Attendance>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Attendance>> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<Attendance>> Update(Attendance item)
        {
            throw new NotImplementedException();
        }
    }
}
