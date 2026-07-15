using GenericRepository;
using RentCarServer.Domain.Users;

namespace RentCarServer.WebAPI
{
    public static class ExtensionMethods
    {
        public static async Task CreateUser(this WebApplication app)
        {
            using var scoped = app.Services.CreateScope();
            var userRepository = scoped.ServiceProvider.GetRequiredService<IUserRepository>();
            var unitOfWork = scoped.ServiceProvider.GetRequiredService<IUnitOfWork>();

            if(!(await userRepository.AnyAsync(p=>p.UserName.Value == "admin")))
            {
                FirstName firstName = new("Serbay");
                LastName lastName = new("Sarıkaya");
                Email email = new("serbaysarikaya@gmail.com");
                UserName userName = new("admin");
                Password password = new("1");

                var user = new User(
                    firstName,
                    lastName,
                    email,
                    userName,
                    password);

                userRepository.Add(user);
                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}
