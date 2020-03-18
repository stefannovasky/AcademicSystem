using DAL.Interfaces;
using Entities;
using log4net;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Impl
{
    public class EvaluationRepository : IEvaluationRepository
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private AcademyContext _context;
        public EvaluationRepository(AcademyContext context)
        {
            _context = context;
        }
        public async Task<Response> Create(Evaluation item)
        {
            Response response = new Response();
            try
            {
                item.Student = null;
                item.Class = null;

                item.CreatedAt = DateTime.Now;
                await _context.Evaluations.AddAsync(item);
                await _context.SaveChangesAsync();
                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                if (e.Message.Contains("Value"))
                {
                    response.ErrorList.Add("Value is required.");
                }
                if (e.Message.Contains("Name"))
                {
                    response.ErrorList.Add("Name is required.");
                }
                else
                {
                    response.ErrorList.Add("Error while adding Evaluation.");
                }
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                return response;
            }
        }

        public async Task<Response> Delete(int id)
        {
            Response response = new Response();
            try
            {

                Evaluation Evaluation = await _context.Evaluations.FindAsync(id);
                Evaluation.IsActive = false;
                Evaluation.DeletedAt = DateTime.Now;
                _context.Evaluations.Update(Evaluation);
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Invalid Evaluation Id");
                return response;
            }
        }

        public async Task<DataResponse<Evaluation>> GetAll()
        {
            DataResponse<Evaluation> response = new DataResponse<Evaluation>();
            try
            {

                response.Data = await _context.Evaluations.Where(a => a.IsActive == true).ToListAsync();
                return response;

            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Error while getting Evaluations.");
                return response;
            }
        }

        public async Task<DataResponse<Evaluation>> GetByID(int id)
        {
            DataResponse<Evaluation> response = new DataResponse<Evaluation>();
            try
            {

                response.Data.Add(await _context.Evaluations.Include(c => c.Class).Include(c => c.Student).SingleOrDefaultAsync(e => e.IsActive && e.ID == id));
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Error while getting Evaluation.");
                return response;
            }
        }

        public async Task<DataResponse<Evaluation>> Update(Evaluation item)
        {
            DataResponse<Evaluation> response = new DataResponse<Evaluation>();
            try
            {

                item.UpdatedAt = DateTime.Now;
                _context.Evaluations.Update(item);
                await _context.SaveChangesAsync();
                return response;

            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                log.Error(sb.AppendLine(e.Message).AppendLine(e.StackTrace).ToString());
                response.Success = false;
                response.ErrorList.Add("Error while updating Evaluation.");
                return response;
            }
        }
    }
}
