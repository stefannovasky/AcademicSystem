﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AcademicSystemApi.Extensions;
using BLL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace AcademicSystemApi.Controllers
{
    [Route("students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        IStudentService _service;
        IUserService userService;
        IInstructorService InstructorService;
        ICoordinatorService CoordinatorService;
        IOwnerService OwnerService;
        ICourseService CourseService;
        public StudentController(IStudentService service, IUserService userService, IInstructorService InstructorService, ICoordinatorService coordinatorService, IOwnerService ownerService, ICourseService courseService)
        {
            this._service = service;
            this.userService = userService;
            this.InstructorService = InstructorService;
            this.CoordinatorService = coordinatorService;
            this.OwnerService = ownerService;
            this.CourseService = courseService;
        }

        [Authorize]
        public async Task<DataResponse<Student>> GetStudents()
        {
            try
            {
                DataResponse<Student> response = await _service.GetAll();
                foreach (Student student in response.Data)
                {
                    if (student.User != null)
                        student.User.Student = null;
                }
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<DataResponse<Student>> GetStudent(int id)
        {
            try
            {
                bool isPermited = false;
                DataResponse<Student> studentResponse = (await _service.GetByID(id));
                if (studentResponse.Success)
                {
                    Student student = studentResponse.Data[0];
                    isPermited = await PermisionCheckStudentInClass(student);
                }
                if (isPermited)
                {
                    return studentResponse;
                }
                DataResponse<Student> response = new DataResponse<Student>();
                response.Success = false;
                response.ErrorList.Add("Permission Denied");
                return response;
            } catch (Exception e)
            {
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<object> CreateStudent(Student student)
        {
            try
            {
                Response response = await _service.Create(student);
                return new
                {
                    sucess = response.Success
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<Response> UpdateStudent(Student student)
        {
            try
            {
                Response response = await _service.Update(student);
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<Response> DeleteStudent(int id)
        {
            try
            {
                Response response = await _service.Delete(id);
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        private async Task<bool> PermisionCheckStudentInClass(Student student)
        {
            bool isPermited = false;
            User user = (await userService.GetByID(this.GetUserID())).Data[0];
            if (user.Student != null && user.Student.ID == student.ID)
            {
                isPermited = true;
            }
            if (user.Instructor != null)
            {
                Instructor instructor = (await InstructorService.GetByID(user.Instructor.ID)).Data[0];
                foreach (InstructorClass instructorClass in instructor.Classes)
                {
                    if (student.Classes.Where(ic => ic.ClassID == instructorClass.ClassID).Count() > 0)
                    {
                        isPermited = true;
                    }
                }
            }
            if (user.Coordinator != null)
            {
                Coordinator Coordinator = (await CoordinatorService.GetByID(user.Coordinator.ID)).Data[0];

                foreach (CoordinatorClass CoordinatorClass in Coordinator.Classes)
                {
                    if (student.Classes.Where(ic => ic.ClassID == CoordinatorClass.ClassID).Count() > 0)
                    {
                        isPermited = true;
                    }
                }
            }
            if (user.Owner != null)
            {
                Owner Owner = (await OwnerService.GetByID(user.Owner.ID)).Data[0];
                foreach (OwnerCourse ownerCourse in Owner.Courses)
                {
                    Course course = (await CourseService.GetByID(ownerCourse.CourseID)).Data[0];
                    foreach (Class @class in course.Classes)
                    {
                        if (student.Classes.Where(sc => sc.ClassID == @class.ID).Count() > 0)
                        {
                            isPermited = true;
                        }
                    }
                }
            }
            return isPermited;
        }
    }
}