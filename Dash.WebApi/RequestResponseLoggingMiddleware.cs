using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace anyhelp.Api
{

    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
    public class RequestResponseLoggingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        public RequestResponseLoggingMiddleware(RequestDelegate next,
                                                ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory
                      .CreateLogger<RequestResponseLoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }
        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            //_logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
            //                       $"Schema:{context.Request.Scheme} " +
            //                       $"Host: {context.Request.Host} " +
            //                       $"Path: {context.Request.Path} " +
            //                       $"QueryString: {context.Request.QueryString} " +
            //                       $"Request Body: {ReadStreamInChunks(requestStream)}");
            string log=            Environment.NewLine
            +$"Http Request Information:{Environment.NewLine}" + Environment.NewLine +
                  $"Datetime: {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")} " + Environment.NewLine +
                       $"Schema:{context.Request.Scheme} " + Environment.NewLine +
                       $"Host: {context.Request.Host} " + Environment.NewLine +
                       $"Path: {context.Request.Path} " + Environment.NewLine +
                       $"QueryString: {context.Request.QueryString} " + Environment.NewLine +
                       $"Request Body: {ReadStreamInChunks(requestStream)}"+ Environment.NewLine ;

            var path = AppDomain.CurrentDomain.BaseDirectory+"Log";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); 
            }
            try
            {
                var pathfile = AppDomain.CurrentDomain.BaseDirectory + "Log//log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(pathfile))
                { // Create a file to write to   
                    File.WriteAllText(pathfile, log);
                }
                else
                {
                    File.AppendAllText(pathfile, log);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }

            context.Request.Body.Position = 0;
        }
        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }
        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            await _next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            //_logger.LogInformation($"Http Response Information:{Environment.NewLine}" +
            //                       $"Schema:{context.Request.Scheme} " +
            //                       $"Host: {context.Request.Host} " +
            //                       $"Path: {context.Request.Path} " +
            //                       $"QueryString: {context.Request.QueryString} " +
            //                       $"Response Body: {text}");
            string log = Environment.NewLine
            + $"Http Response Information:{Environment.NewLine}" + Environment.NewLine+
                 $"Datetime: {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")} " + Environment.NewLine +
                                   $"Schema:{context.Request.Scheme} " + Environment.NewLine +
                                   $"Host: {context.Request.Host} " + Environment.NewLine +
                                   $"Path: {context.Request.Path} " + Environment.NewLine +
                                   $"QueryString: {context.Request.QueryString} " + Environment.NewLine +
                                   $"Response Body: {text}"+Environment.NewLine ;

            var path = AppDomain.CurrentDomain.BaseDirectory + "Log";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                var pathfile = AppDomain.CurrentDomain.BaseDirectory + "Log//log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(pathfile))
                { // Create a file to write to   
                    File.WriteAllText(pathfile, log);
                }
                else
                {
                    File.AppendAllText(pathfile, log);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
