using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RentCarServer.Application.Behaviors;
using TS.MediatR;

namespace RentCarServer.Application
{
    public static class ServiceRegisterar
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfr =>
            {
                cfr.RegisterServicesFromAssembly(typeof(ServiceRegisterar).Assembly);
                cfr.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfr.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(ServiceRegisterar).Assembly);

            return services;
        }
    }
}
