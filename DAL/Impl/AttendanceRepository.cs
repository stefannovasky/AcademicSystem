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
        private AcademyContext _context; 
        public AttendanceRepository(AcademyContext context)
        {
            this._context = context;
        }
        public async Task<Response> Create(Attendance item)
        {
            Response response = new Response();
            try
            {
                item.Student = null;
                item.Class = null;
                
                item.CreatedAt = DateTime.Now;
                await _context.Attendances.AddAsync(item);
                await _context.SaveChangesAsync();
                return response;
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

                Attendance attendance = await _context.Attendances.FindAsync(id);
                attendance.IsActive = false;
                attendance.DeletedAt = DateTime.Now;
                _context.Attendances.Update(attendance);
                await _context.SaveChangesAsync();
                return response;

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

                response.Data = await _context.Attendances.Where(a => a.IsActive == true).ToListAsync();
                return response;

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

                response.Data.Add(await _context.Attendances.Include(a => a.Class).Include(a => a.Student).SingleOrDefaultAsync(a => a.IsActive && a.ID == id));
                return response;

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

                item.UpdatedAt = DateTime.Now;
                _context.Attendances.Update(item);
                await _context.SaveChangesAsync();
                return response;

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
