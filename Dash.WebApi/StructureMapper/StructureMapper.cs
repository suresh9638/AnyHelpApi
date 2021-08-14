using anyhelp.Data.DataContext;
using anyhelp.Service.Interface;
using anyhelp.Service.Service;

using Microsoft.Extensions.DependencyInjection;

namespace anyhelp.WebApi
{
    public static class StructureMapper
    {
        public static void InitializeStructureMapper(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
           
        
        }
    }
}
