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
    [Route("classes")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        IClassService _classService; 
        ICourseService _courseService;
        IUserService _userService;
        IOwnerService _ownerService;
        public CourseController(IClassService classService, ICourseService courseService, IUserService userService, IOwnerService ownerService)
        {
            this._classService = classService; 
            this._courseService = courseService;
            this._userService = userService;
            this._ownerService = ownerService;
        }

      

        [HttpPost]
        [Authorize]
        public async Task<object> CreateClass(Class Class)
            //course
        {
            try
            {
                int coordinatorId = await this.VerifyIfUserIsCoordinatorAndReturnOwnerId();
                if (coordinatorId == 0)
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

        /// <summary>
        /// If user isn't a owner return 0
        /// </summary>
        /// <returns></returns>
        private async Task<int> VerifyIfUserIsCoordinatorAndReturnOwnerId()
        {
            string idstring = HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
            int id = Convert.ToInt32(idstring);

            User user = (await this._userService.GetByID(id)).Data[0];
            if (user.Coordinator != null)
            {
                return user.Coordinator.ID;
            }

            return 0;
        }
    }
}