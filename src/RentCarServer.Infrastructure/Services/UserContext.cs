using Microsoft.AspNetCore.Http;
using RentCarServer.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Infrastructure.Services
{
    internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public Guid GetUserId()
        {
           var httpcontext = httpContextAccessor.HttpContext;
           
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
            catch (Exception )
            {

                throw new ArgumentException("Kullanıcı id uygun formatta değildir.");
            }
        }
    }
}
