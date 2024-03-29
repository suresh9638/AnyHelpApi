﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static anyhelp.Service.Helper.ServiceResponse;

namespace anyhelp.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        public IActionResult GenerateResponse<T>(ServiceResponseGeneric<T> serviceResponse)
        {
            if (serviceResponse.Success)
            {
                return Ok(serviceResponse);
            }
            return HandleHttpStatusCodes(serviceResponse, serviceResponse.StatusCode);
        }
        protected IActionResult GenerateResponse(ExecutionResult result, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            if (result.Success)
            {
                return Ok(result);
            }
            return HandleHttpStatusCodes(result, statusCode);
        }

        /// <summary>
        /// This method is implemented to get DRY from two method FromExecutionResult
        /// </summary>
        private IActionResult HandleHttpStatusCodes(ExecutionResult result, HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.Unauthorized => Unauthorized(result),
                HttpStatusCode.Forbidden => Forbid(),
                _ => BadRequest(result.Errors),
            };
        }

        private IActionResult HandleHttpStatusCodes<T>(ServiceResponseGeneric<T> result, HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.Unauthorized => Unauthorized(result),
                HttpStatusCode.Forbidden => Forbid(),
                _ => BadRequest(result.Errors),
            };
        }
        protected IActionResult FromExecutionResult<T>(ExecutionResult<T> result, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            if (result.Success)
            {
                return Ok(result);
            }
            return HandleHttpStatusCodes(result, statusCode);
        }


    }
}
