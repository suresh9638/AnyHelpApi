using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace anyhelp.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                      .MinimumLevel.Verbose()
                      .Enrich.FromLogContext()
                      .WriteTo.Console(LogEventLevel.Error)
                      .WriteTo.RollingFile("Logs/MainLog-{Date}.log", LogEventLevel.Error)
                      .CreateLogger();

            Log.Information("[MAIN] Starting Application.");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
