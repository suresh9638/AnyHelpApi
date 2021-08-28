using AutoMapper;
using anyhelp.Api;
using anyhelp.Data.DataContext;
using anyhelp.Service;
using anyhelp.Service.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;

namespace anyhelp.WebApi
{
    public class Startup
    {
        private const string PortalName = "Any Help";
        private const string Version1 = "V1";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AppConfiguration appConfiguration = new AppConfiguration();
            //IConfiguration appSettingsSection = Configuration.GetSection("AppSettings");
            //services.Configure<AppSettings>(appSettingsSection);
            //services.AddSession(options => {
            //    options.IdleTimeout = TimeSpan.FromMinutes(1440);
            //});
            //AppSettings appSettings = appSettingsSection.Get<AppSettings>();
            services.AddDbContext<anyhelpContext>(options =>
                                 options.UseSqlServer(appConfiguration.SqlConnectonString));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(Version1, new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = Version1,
                    Title = PortalName,
                    Description = $"{PortalName} v1 APIs",
                });

            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                   // ValidIssuer = appSettings.TokenSettings.validIssuer,
                  //  ValidAudience = appSettings.TokenSettings.validAudience,
                  //  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.TokenSettings.securityKey))
                };
            });
            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme);

                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin(); 
                });
            });
           
            services.AddControllersWithViews();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
           
            services.AddControllers();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperConfiguration());
            });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
         services.AddMvc();
            services.AddDistributedMemoryCache();
            services.AddSession();
           AppConfiguration appseting = new AppConfiguration();
            services.AddDistributedSqlServerCache(options => {
                options.ConnectionString = appseting.SqlConnectonString;
                options.SchemaName = "dbo";
                options.TableName = "MyCache";
                options.DefaultSlidingExpiration = TimeSpan.FromMinutes(1440);
               
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            StructureMapper.InitializeStructureMapper(services);
           
            services.AddSignalR();
            string[] Parameters = { "http://anyhelp.in.net", "http://api.anyhelp.in.net", "https://anyhelp.in.net", "https://api.anyhelp.in.net", "https://localhost:4200", "http://localhost:4200", "http://localhost:44327", "https://localhost:44327" };
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => { builder.AllowAnyMethod().AllowAnyHeader().WithOrigins(Parameters); }));

        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AppConfiguration appConfiguration = new AppConfiguration();
            string[] Parameters = { "http://anyhelp.in.net", "http://api.anyhelp.in.net", "https://anyhelp.in.net", "https://api.anyhelp.in.net", "https://localhost:4200", "http://localhost:4200", "http://localhost:44327", "https://localhost:44327" };
            app.UseCors(x => x
            .WithOrigins(Parameters)
                    .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin());                    ;
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
               
                app.UseHsts();
            }

            app.UseStaticFiles();

            //enable session before MVC
            app.UseSession();
           
            app.UseRequestResponseLogging();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=sample}/{action=Index}/{id?}");
            });
            //app.UseEndpoints(endpoints => {
            //    endpoints.MapHub<NotificationHub>("/location");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NET5SignalR.Models.NotificationHub>("/notification-hub");
            });
            // Enable middle ware to serve generated Swagger as a JSON endpoint.  
            app.UseSwagger();

            // Enable middle ware to serve swagger-ui (HTML, JS, CSS, etc.),  r
            // specifying the Swagger JSON endpoint.  
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{Version1}/swagger.json", Version1);
                c.DisplayOperationId();
                c.DisplayRequestDuration();
            });

            if(Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"Document")))
                    {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), @"Document")),
                    RequestPath = new PathString("/Document")
                });

                app.UseDirectoryBrowser(new DirectoryBrowserOptions()
                {
                    FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), @"Document")),
                    RequestPath = new PathString("/Document")
                });
            }


        }
    }
}
