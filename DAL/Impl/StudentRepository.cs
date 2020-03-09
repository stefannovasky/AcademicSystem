﻿using DAL.Interfaces;
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
    public class StudentRepository : IStudentRepository
    {
        public async Task<Response> Create(Student item)
        {
            try
            {
                item.User = null;
                using (AcademyContext ctx = new AcademyContext())
                {
                    await ctx.Students.AddAsync(item);
                    await ctx.SaveChangesAsync();
                }

                return new Response() { Success = true };
            }
            catch (Exception ex)
            {
                Response r = new Response() { Success = false };

                if (ex.InnerException.Message.Contains("unique index"))
                {
                    r.ErrorList.Add("Student already exists");
                } else
                {
                    r.ErrorList.Add("Insert student error");
                }

                return r;
            }
        }

        public async Task<Response> Delete(int id)
        {
            try
            {
                using (AcademyContext ctx = new AcademyContext())
                {
                    Student u = await ctx.Students.FindAsync(id);
                    u.IsActive = false;
                    u.DeletedAt = DateTime.Now; 
                    ctx.Update(u);
                    await ctx.SaveChangesAsync();
                }
                return new Response();
            }
            catch (Exception ex)
            {
                Response r = new Response() { Success = false };
                r.ErrorList.Add("Delete student error");
                return r;
            }
        }

        public async Task<DataResponse<Student>> GetAll()
        {
            try
            {
                List<Student> students = new List<Student>();

                using (AcademyContext ctx = new AcademyContext())
                {
                    students = await ctx.Students
                        .Where(u => u.IsActive == true)
                        .ToListAsync();
                }
                DataResponse<Student> r = new DataResponse<Student>();
                r.Data = students;

                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Student> r = new DataResponse<Student>() { Success = false };
                r.ErrorList.Add("Get all students error");
                return r;
            }
        }

        public async Task<DataResponse<Student>> GetByID(int id)
        {
            try
            {
                Student student = new Student();

                using (AcademyContext ctx = new AcademyContext())
                {
                    student = await ctx.Students.Include(u => u.User).SingleOrDefaultAsync(u => u.IsActive == true && u.ID == id);
                }
                if (student == null)
                {
                    DataResponse<Student> response = new DataResponse<Student>() { Success = false };
                    response.ErrorList.Add("User not found");
                    return response;
                }

                DataResponse<Student> r = new DataResponse<Student>();
                r.Data.Add(student);

                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Student> r = new DataResponse<Student>() { Success = false };
                r.ErrorList.Add("Get student error");
                return r;
            }
        }

        public async Task<DataResponse<Student>> Update(Student item)
        {
            try
            {
                Student u = new Student();
                using (AcademyContext ctx = new AcademyContext())
                {
                    u = await ctx.Students.AsNoTracking().SingleOrDefaultAsync(u => u.ID == item.ID);
                    u = item;
                    u.UpdatedAt = DateTime.Now; 
                    ctx.Update(u);
                    await ctx.SaveChangesAsync();
                }

                DataResponse<Student> r = new DataResponse<Student>();
                r.Data.Add(u);
                return r;
            }
            catch (Exception ex)
            {
                DataResponse<Student> r = new DataResponse<Student>() { Success = false };
                r.ErrorList.Add("Update student error");
                return r;
            }
        }
    }
}
