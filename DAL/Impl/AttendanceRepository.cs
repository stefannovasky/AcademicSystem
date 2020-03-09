using DAL.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Impl
{
    public class AttendanceRepository : IAttendanceRepository
    {
        public async Task<Response> Create(Attendance item)
        {
            Response response = new Response();
            try
            {
                item.Student = null;
                item.Class = null;
                using (AcademyContext context = new AcademyContext())
                {
                    item.CreatedAt = DateTime.Now;
                    await context.Attendances.AddAsync(item);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                if (e.Message.Contains("Date")) 
                {
                    response.ErrorList.Add("Date is required.");
                }
                if (e.Message.Contains("Value"))
                {
                    response.ErrorList.Add("Value is required.");
                }
                else 
                {
                    response.ErrorList.Add("Error while adding Attendance.");
                }
                return response;
            }
        }

        public async Task<Response> Delete(int id)
        {
            Response response = new Response();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    Attendance attendance =  await context.Attendances.FindAsync(id);
                    attendance.IsActive = false;
                    attendance.DeletedAt = DateTime.Now;
                    context.Attendances.Update(attendance);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Invalid Attendance Id");
                return response;
            }
        }

        public async Task<DataResponse<Attendance>> GetAll()
        {
            DataResponse<Attendance> response = new DataResponse<Attendance>();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    response.Data = await context.Attendances.Where(a => a.IsActive == true).ToListAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while getting Attendances.");
                return response;
            }
        }

        public async Task<DataResponse<Attendance>> GetByID(int id)
        {
            DataResponse<Attendance> response = new DataResponse<Attendance>();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    response.Data.Add(await context.Attendances.FindAsync(id));
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while getting Attendance.");
                return response;
            }
        }

        public async Task<DataResponse<Attendance>> Update(Attendance item)
        {
            DataResponse<Attendance> response = new DataResponse<Attendance>();
            try
            {
                using (AcademyContext context = new AcademyContext())
                {
                    item.UpdatedAt = DateTime.Now;
                    context.Attendances.Update(item);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while updating attendance.");
                return response;
            }
        }
    }
}
