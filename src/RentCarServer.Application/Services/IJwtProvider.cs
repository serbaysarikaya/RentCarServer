using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Application.Services
{
    public interface IJwtProvider
    {
        Task<string> CreateTokenAsync(User user,CancellationToken cancellationToken=default);
    }
}
