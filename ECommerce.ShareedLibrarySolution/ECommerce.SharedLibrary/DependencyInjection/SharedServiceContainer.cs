using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ECommerce.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services,IConfiguration config , string fileName)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                config.GetConnectionString("eCommerceConnection"), sqlserverOption => sqlserverOption.EnableRetryOnFailure()));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{New:ine}{Exception}",
                rollingInterval: RollingInterval.Day).CreateLogger();
                JWTAuthenticationSchema.AddJWTAuthenticationSchena(services, config);
            return services;
        }
        public static IApplicationBuilder UseSharedServices(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalException>();
            //app.UseMiddleware<ListenToOnlyApiGateway>();

            return app;
        }
    }
}
