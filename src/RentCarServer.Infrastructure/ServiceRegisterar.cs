using FluentEmail.MailKitSmtp;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentCarServer.Infrastructure.Context;
using RentCarServer.Infrastructure.Options;
using Scrutor;
namespace RentCarServer.Infrastructure
{
    public static class ServiceRegisterar
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.ConfigureOptions<JwtSetupOptions>();
            services.AddAuthentication().AddJwtBearer();
            services.AddAuthorization();
            services.Configure<MailSettingOptions>(configuration.GetSection("MailSettings"));
            // BuildServiceProvider() anti-pattern'i kaldırıldı, direkt configuration'dan okunuyor
            var mailSettings = configuration.GetSection("MailSettings").Get<MailSettingOptions>()!;
            if (string.IsNullOrEmpty(mailSettings.UserId))
            {
                services.AddFluentEmail(mailSettings.Email)
                    .AddMailKitSender(new SmtpClientOptions
                    {
                        Server = mailSettings.Smtp,
                        Port = mailSettings.Port,
                        UseSsl = mailSettings.SSL,
                        RequiresAuthentication = false
                    });
            }
            else
            {
                services.AddFluentEmail(mailSettings.Email)
                    .AddMailKitSender(new SmtpClientOptions
                    {
                        Server = mailSettings.Smtp,
                        Port = mailSettings.Port,
                        User = mailSettings.UserId,
                        Password = mailSettings.Password,
                        UseSsl = mailSettings.SSL,
                        RequiresAuthentication = true
                    });
            }
            services.AddHttpContextAccessor();
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                string con = configuration.GetConnectionString("SqlServer")!;
                opt.UseSqlServer(con);
            });
            services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());
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
                .WithScopedLifetime());
            return services;
        }
    }
}