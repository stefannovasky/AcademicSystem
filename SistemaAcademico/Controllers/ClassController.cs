using System;
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
using Newtonsoft.Json;
using Shared;

namespace AcademicSystemApi.Controllers
{
    [Route("classes")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        IClassService _classService;
        ICourseService _courseService;
        IUserService _userService;
        IOwnerService _ownerService;
        ICoordinatorService _coordinatorService;
        IInstructorService _instructorService;
        public ClassController(IClassService classService, ICourseService courseService, IUserService userService, IOwnerService ownerService, ICoordinatorService coordinatorService, IInstructorService instructorService)
        {
            this._classService = classService;
            this._courseService = courseService;
            this._userService = userService;
            this._ownerService = ownerService;
            this._coordinatorService = coordinatorService;
            this._instructorService = instructorService;
        }

        [HttpPost]
        [Authorize]
        public async Task<object> Create(Class Class)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Owner != null)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];

                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == Class.CourseID).Any())
                        {
                            Response response = await _classService.Create(Class);
                            return new
                            {
                                success = response.Success
                            };
                        }
                    }
                }

                return Forbid();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /*
        [Authorize]
        public async Task<object> GetAll()
        { 
            try
            {
                DataResponse<Class> response = await _classService.GetAll();


                return new
                {
                    success = response.Success,
                    data = response.Success ? response.Data : null
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }
        */

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetClass(int id)
        {
            try
            {
                DataResponse<Class> response = await _classService.GetByID(id);

                // verify 
                Class Class = (await _classService.GetByID(id)).Data[0];
                if (await this.PermissionCheckToReadClass(Class))
                {
                    return this.SendResponse(response);
                }

                return Forbid();
            }
            catch (Exception e)
            {
                return Forbid();
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<object> DeleteClass(int id)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Owner != null)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];
                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == id).Any())
                        {
                            Response response = await _classService.Delete(id);
                            return new
                            {
                                success = response.Success
                            };
                        }
                    }
                }

                return Forbid(); 
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("student")]
        public async Task<object> AddStudent([FromBody] StudentClass item)
        {
            try
            {
                bool isPermited = false; 
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Owner != null)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];
                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == item.ClassID).Any())
                        {
                            isPermited = true;
                        }
                    }
                }
                if (!isPermited)
                {
                    if (user.Instructor != null)
                        {
                            Instructor instructor = (await this._instructorService.GetByID(user.Instructor.ID)).Data[0];
                            if (instructor.Classes.Where(ic => ic.ClassID == item.ClassID).Any())
                            {
                                isPermited = true; 
                            }
                        }
                    }
                if (!isPermited)
                {
                    if (user.Coordinator != null)
                    {
                        Coordinator coordinator = (await this._coordinatorService.GetByID(user.Instructor.ID)).Data[0];
                        if (coordinator.Classes.Where(cc => cc.ClassID == item.ClassID).Any())
                        {
                            isPermited = true;
                        }
                    }
                }

                if (isPermited)
                {
                    Response response = await _classService.AddStudent(new Class() { ID = item.ClassID }, new Student() { ID = item.StudentID });
                    return new
                    {
                        success = response.Success
                    };
                }

                return Forbid();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("instructor")]
        public async Task<object> AddInstructor([FromBody] InstructorClass item)
        {
            try
            {
                bool isPermited = false; 
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];
                
                if (user.Owner == null && user.Coordinator == null)
                {
                    return Forbid();
                }

                Coordinator coordinator = (await _coordinatorService.GetByID(user.Coordinator.ID)).Data[0];
                if (coordinator.Classes.Where(c => c.ClassID == item.ClassID).Any()) 
                {
                    isPermited = true; 
                }

                if (!isPermited)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];
                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == item.ClassID).Any())
                        {
                            isPermited = true;
                        }
                    }
                }

                if (isPermited)
                {
                    Response response = await _classService.AddInstructor(new Class() { ID = item.ClassID }, new Instructor() { ID = item.InstructorID });
                    return new
                    {
                        success = response.Success
                    };
                }

                return Forbid();
            }
            catch (Exception e)
            {
                return null;
            }
        }


        [Authorize]
        [HttpPost]
        [Route("coordinator")]
        public async Task<object> AddCoordinator([FromBody] CoordinatorClass item)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Owner != null)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];
                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == item.ClassID).Any())
                        {
                            Response response = await _classService.AddCoordinator(new Class() { ID = item.ClassID }, new Coordinator() { ID = item.CoordinatorID });
                            return new
                            {
                                success = response.Success
                            };
                        }
                    }
                }

                return Forbid();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        

        private async Task<bool> PermissionCheckToReadClass(Class Class)
        {
            bool hasPermissionToRead = false;
            int userID = this.GetUserID();
            User u = (await this._userService.GetByID(userID)).Data[0];

            if (u.Student != null)
            {
                if (Class.Students != null)
                {
                    foreach (StudentClass student in Class.Students)
                    {
                        if (student.StudentID == u.Student.ID)
                        {
                            hasPermissionToRead = true;
                        }
                    }
                }
            }
            if (!hasPermissionToRead)
            {
                if (Class.Instructors != null)
                {
                    foreach (InstructorClass instructor in Class.Instructors)
                    {
                        if (instructor.InstructorID == u.Instructor.ID)
                        {
                            hasPermissionToRead = true;
                        }
                    }
                }
            }
            if (!hasPermissionToRead)
            {
                Coordinator coordinator = (await this._coordinatorService.GetByID(u.Coordinator.ID)).Data[0];
                if (Class.Coordinators != null)
                {
                    foreach (CoordinatorClass coordinatorClass in Class.Coordinators)
                    {
                        if ((coordinator.Classes.Where(c => c.ClassID == Class.ID).ToList().Count) > 0)
                        {
                            hasPermissionToRead = true;
                        }
                    }
                }
            }
            return hasPermissionToRead;
        }

        [Authorize]
        [HttpPut]
        [Route("")]
        public async Task<object> Update(Class Class)
        {
            try
            {
                User user = (await this._userService.GetByID(this.GetUserID())).Data[0];

                if (user.Owner != null)
                {
                    Owner owner = (await _ownerService.GetByID(user.Owner.ID)).Data[0];

                    foreach (OwnerCourse oc in owner.Courses)
                    {
                        Course course = (await _courseService.GetByID(oc.CourseID)).Data[0];
                        if (course.Classes.Where(c => c.ID == Class.CourseID).Any())
                        {
                            Response response = await _classService.Update(Class);
                            return new
                            {
                                success = response.Success
                            };
                        }
                    }
                }

                return Forbid();
            }
            catch (Exception)
            {
                Response.StatusCode = StatusCode(500).StatusCode;
                return null; 
            }
        }



        /*
        private async Task<bool> PermissionCheckToAddClass(Class Class) 
        {
            // Ser um dos coordinator do curso em que a classe está sendo cadastrada 
            //    class Course 
            //     Co
            //Ser owner do curso em que a classe esta sendo cadastrada

            bool hasPermissionToRead = false;
            int userID = this.GetUserID();
            User u = (await this._userService.GetByID(userID)).Data[0];

            if (u.Owner != null)
            {
                Owner owner = (await _ownerService.GetByID(u.Owner.ID)).Data[0];
                foreach (OwnerCourse ownerCourse in owner.Courses)
                {
                    if (ownerCourse.CourseID == Class.CourseID)
                    {
                        hasPermissionToRead = true;
                    }
                }
            }

            if (u.Coordinator != null && Class.Coordinators != null)
            {
                //Coordinator coordinator = (await _coordinatorService.GetByID(u.Coordinator.ID)).Data[0];

                //Course course = (await _courseService.GetByID(Class.CourseID)).Data[0];
                
                foreach (var coordinator in Class.Coordinators)
                {
                    if (coordinator.ClassID == Class.ID)
                    {
                        hasPermissionToRead = true;
                    }
                }
            }

            return true; 
        }*/
    }
}