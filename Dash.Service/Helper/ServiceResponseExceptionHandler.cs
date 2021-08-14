using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace anyhelp.Service.Helper
{
    public class ServiceResponseExceptionHandler<T> : Exception
    {
        public ILogger<T> _logger;
        public string Exception { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }

        public ServiceResponseExceptionHandler(ILogger<T> logger)
        {
            _logger = logger;
        }
        public ServiceResponseExceptionHandler(string msg, HttpStatusCode httpStatusCode)
        {
            Exception = msg;
            HttpStatusCode = httpStatusCode;

        }

    }
}
