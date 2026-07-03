using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentCarServer.Infrastructure.Context;
using Scrutor;
using TS.MediatR;

namespace RentCarServer.Infrastructure
{
    public static class ServiceRegisterar
    {

        public static IServiceCollection Addnfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                string con = configuration.GetConnectionString("SqlServer")!;
                opt.UseSqlServer(con);
            });

            services.Scan(action => action
          .FromAssemblies(typeof(ServiceRegisterar).Assembly)
          .AddClasses(classes => classes
              .Where(type =>
                  type.IsClass &&
                  !type.IsAbstract &&
                  !typeof(Delegate).IsAssignableFrom(type) &&
                  !typeof(DbContext).IsAssignableFrom(type)),
              publicOnly: false)
          .UsingRegistrationStrategy(RegistrationStrategy.Skip)
          .AsImplementedInterfaces()
          .WithScopedLifetime()
      );
            return services;
        }
    }
}
