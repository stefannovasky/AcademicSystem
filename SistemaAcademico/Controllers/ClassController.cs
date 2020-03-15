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
        public ClassController(IClassService classService, ICourseService courseService, IUserService userService, IOwnerService ownerService)
        {
            this._classService = classService;
            this._courseService = courseService;
            this._userService = userService;
            this._ownerService = ownerService;
        }

        [HttpPost]
        [Authorize]
        public async Task<object> Create(Class Class)
        {
            try
            {
                int coordinatorID = await this.VerifyIfUserIsCoordinatorAndReturnOwnerId();
                if (coordinatorID == 0)
                {
                    return Forbid();
                }


                DataResponse<int> response = await _classService.CreateAndReturnId(Class);

                if (response.HasError())
                {
                    return new
                    {
                        success = response.Success,
                        message = response.GetErrorMessage()
                    };
                }

                await this._courseService.AddClass(new Course() { ID = Class.CourseID }, new Class() { ID = response.Data[0] });

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

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<object> GetClass(int id)
        {
            try
            {
                DataResponse<Class> response = await _classService.GetByID(id);

                response.Data[0].Course.Classes = null;
                response.Data[0].Subject.Classes = null;
                
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

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<object> DeleteClass(int id)
        {
            try
            {
                Response response = await _classService.Delete(id);
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