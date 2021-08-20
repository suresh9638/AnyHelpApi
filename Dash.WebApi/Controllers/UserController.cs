
using anyhelp.Data.DataContext;
using anyhelp.Service.Interface;
using anyhelp.Service.Models;
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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
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

        [HttpGet]
        [Route("GetAllInquiry")]
        public async Task<IActionResult> GetAllInquiry()
        {

            return GenerateResponse(await _userService.GetAllInquiry());

        }
        [HttpGet]
        [Route("GetAllInquiry1")]
        public async Task<IActionResult> GetAllInquiry1()
        {

            return GenerateResponse(await _userService.GetAllInquiry1());

        }
        [HttpPost]
        [Route("CreateToken")]
        public async Task<IActionResult> CreateToken(UserCreateTokenModel Model)
        {

            return GenerateResponse(await _userService.CreateToken(Model));

        }
    }
}
