
using anyhelp.Data.DataContext;
using anyhelp.Service.Interface;
using anyhelp.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

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
        [Route("GetAllNotification")]
        public async Task<IActionResult> GetAllNotification(string id_token)
        {

            return GenerateResponse(await _userService.GetAllNotification(id_token));

        }
        [HttpGet]
        [Route("GetSearchCaltegory")]
        public async Task<IActionResult> GetSearchCaltegory(string Search)
        {

            return GenerateResponse(await _userService.GetSearchCaltegory(Search));

        }
        [HttpPost]
        [Route("CreateToken")]
        public async Task<IActionResult> CreateToken(UserCreateTokenModel Model)
        {

            return GenerateResponse(await _userService.CreateToken(Model));

        }
        [HttpPost]
        [Route("CreateInquiry")]
        public async Task<IActionResult> CreateInquiry(CreateInquiryModel model)
        {

            return GenerateResponse(await _userService.CreateInquiry(model));

        }

        [HttpGet]
        [Route("SetReadNotification")]
        public async Task<IActionResult> SetReadNotification(long Notificationid, bool Isserivce)
        {

            return GenerateResponse(await _userService.SetReadNotification(Notificationid, Isserivce));

        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> test(string text)
        {

            return Ok(Generatehash512( text));

        }

        [NonAction]
        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }
    }
}
