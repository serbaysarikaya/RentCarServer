using Microsoft.AspNetCore.Http;
using RentCarServer.Application.Services;
using System.Security.Claims;

namespace RentCarServer.Infrastructure.Services
{
    internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public Guid GetUserId()
        {
            var httpcontext = httpContextAccessor.HttpContext;
            if (httpcontext == null)
            {
                throw new ArgumentNullException("Context bilgisi bunumamadı."); ;
            }
            var claims = httpcontext.User.Claims;
            string? userId = claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new ArgumentException("Kullanıcı bilgisi bulunamadı");
            }
            try
            {
                Guid id = Guid.Parse(userId);
                return id;
            }
            catch (Exception)
            {

                throw new ArgumentException("Kullanıcı id uygun formatta değildir.");
            }
        }
    }
}
