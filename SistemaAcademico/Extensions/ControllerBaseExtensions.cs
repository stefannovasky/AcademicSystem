using BLL.Impl;
using BLL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AcademicSystemApi.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static int GetUserID(this ControllerBase controller)
        {
            string id = controller.HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
            return Convert.ToInt32(id);
        }

        /// <summary>
        /// If user isn't a owner return 0
        /// </summary>
        /// <returns></returns>
        public async static Task<int> VerifyIfUserIsOwnerAndReturnOwnerId(this ControllerBase controller, IUserService _userService)
        {
            string idstring = controller.HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
            int id = Convert.ToInt32(idstring);

            User user = (await _userService.GetByID(id)).Data[0];
            if (user.Owner != null)
            {
                return user.Owner.ID;
            }

            return 0;
        }

        /// <summary>
        /// If user isn't a coordinator return 0
        /// </summary>
        /// <returns></returns>
        public async static Task<int> VerifyIfUserIsInstructorAndReturnInstructorId(this ControllerBase controller, IUserService _userService)
        {
            string idstring = controller.HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
            int id = Convert.ToInt32(idstring);

            User user = (await _userService.GetByID(id)).Data[0];
            if (user.Instructor != null)
            {
                return user.Instructor.ID;
            }

            return 0;
        }

        /// <summary>
        /// If user isn't a coordinator return 0
        /// </summary>
        /// <returns></returns>
        public async static Task<int> VerifyIfUserIsCoordinatorAndReturnCoordinatorId(this ControllerBase controller, IUserService _userService)
        {
            string idstring = controller.HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
            int id = Convert.ToInt32(idstring);

            User user = (await _userService.GetByID(id)).Data[0];
            if (user.Coordinator != null)
            {
                return user.Coordinator.ID;
            }

            return 0;
        }
    }
}
