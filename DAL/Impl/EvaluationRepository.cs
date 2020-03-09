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
    public class EvaluationRepository : IEvaluationRepository
    {
        public async Task<Response> Create(Evaluation item)
        {
            Response response = new Response();
            try
            {
                item.Student = null;
                item.Class = null;
                using (AcademyContext context = new AcademyContext())
                {
                    item.CreatedAt = DateTime.Now;
                    await context.Evaluations.AddAsync(item);
                    await context.SaveChangesAsync();
                    return response;
                }
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
                    Evaluation Evaluation = await context.Evaluations.FindAsync(id);
                    Evaluation.IsActive = false;
                    Evaluation.DeletedAt = DateTime.Now;
                    context.Evaluations.Update(Evaluation);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
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
                using (AcademyContext context = new AcademyContext())
                {
                    response.Data = await context.Evaluations.Where(a => a.IsActive == true).ToListAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
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
                using (AcademyContext context = new AcademyContext())
                {
                    response.Data.Add(await context.Evaluations.FindAsync(id));
                    return response;
                }
            }
            catch (Exception e)
            {
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
                using (AcademyContext context = new AcademyContext())
                {
                    item.UpdatedAt = DateTime.Now;
                    context.Evaluations.Update(item);
                    await context.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorList.Add("Error while updating Evaluation.");
                return response;
            }
        }
    }
}
