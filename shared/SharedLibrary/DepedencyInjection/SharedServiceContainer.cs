using SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>
            (this IServiceCollection services, IConfiguration config, string fileName) where TContext : DbContext
        {
            //Add Generic Database context
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                config
                .GetConnectionString("eCommerceConnection"), sqlserverOption => sqlserverOption.EnableRetryOnFailure()));

            //config serilog logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //Add JWT authentication Scheme
            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, config);
            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            //use global exception
            app.UseMiddleware<GlobalException>();

            //Register middleware to block all outsiders API calls
            //app.UseMiddleware<ListenToOnlyApiGateway>();

            return app;
        }
    }
}