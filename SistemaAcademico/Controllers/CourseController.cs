using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace AcademicSystemApi.Controllers
{
    [Route("courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {        
        ICourseService _courseService;
        IUserService _userService;
        IOwnerService _ownerService; 
        public CourseController(ICourseService service, IUserService userService, IOwnerService ownerService)
        {
            this._courseService = service;
            this._userService = userService;
            this._ownerService = ownerService;
        }

        [Authorize]
        public async Task<object> GetCourses()
        {
            try
            {
                DataResponse<Course> response = await _courseService.GetAll();
                
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

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetCourse(int id)
        {
            try
            {
                DataResponse<Course> response = await _courseService.GetByID(id);

                foreach (var course in response.Data)
                {
                    foreach (var owner in course.Owners)
                    {
                        owner.Course = null;
                    }
                }

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

        [HttpPost]
        [Authorize]
        public async Task<object> CreateCourse(Course Course)
        {
            try
            {
                int ownerId = await this.VerifyIfUserIsOwnerAndReturnOwnerId();
                if (ownerId == 0)
                {
                    return Forbid(); 
                }

                
                DataResponse<int> response = await _courseService.CreateAndReturnId(Course);

                if (response.HasError())
                {
                    return new
                    {
                        success = response.Success,
                        message = response.GetErrorMessage()
                    };
                }

                await this._ownerService.AddCourse(new Owner() { ID = ownerId }, new Course() { ID = response.Data[0] });

                return new
                {
                    success = response.Success
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<object> UpdateCourse(Course Course, int id)
        {
            try
            {
                int ownerID = await this.VerifyIfUserIsOwnerAndReturnOwnerId();
                if (ownerID == 0)
                {
                    return Forbid();
                }
                Course.ID = id; 
                Response response = await _courseService.Update(Course);
                return new
                {
                    success = response.Success
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<object> DeleteCourse(int id)
        {
            try
            {
                int ownerID = await this.VerifyIfUserIsOwnerAndReturnOwnerId();
                if (ownerID == 0)
                {
                    return Forbid();
                }

                Response response = await _courseService.Delete(id);
                return new
                {
                    success = response.Success
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// If user isn't a owner return 0
        /// </summary>
        /// <returns></returns>
        private async Task<int> VerifyIfUserIsOwnerAndReturnOwnerId()
        {
            string idstring = HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
            int id = Convert.ToInt32(idstring);
            
            User user = (await this._userService.GetByID(id)).Data[0];
            if (user.Owner != null)
            {
                return user.Owner.ID; 
            }

            return 0; 
        }
    }
}