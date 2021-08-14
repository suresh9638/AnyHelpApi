
using anyhelp.Data.DataContext;
using anyhelp.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace anyhelp.WebApi.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        //public UserController(IUserService userService)
        //{
        //    _userService = userService;
        //}
        //[HttpGet]
        //[Route("GetUsers")]
        //public IActionResult GetUsers()
        //{
        //    return GenerateResponse( _userService.GetUser());
        //}

        //[HttpGet]
        //[Route("UpdateUserStatus")]
        //public IActionResult UpdateUserStatus(long id)
        //{
        //    return GenerateResponse( _userService.UpdateUserStatus(id));
        //}
        //[HttpGet]
        //[Route("UserLogout")]
        //public IActionResult UserLogout(long UserId)
        //{
        //    return GenerateResponse( _userService.UserLogout(UserId));
        //}

        //[HttpGet]
        //[Route("AdminLogin")]
        //public async Task<IActionResult> LoginForAdmin(string EmailId, string Password)
        //{

        //    return GenerateResponse(await _RegistrationService.AdminLogin(EmailId, Password));

        //}
    }
}
