using Microsoft.AspNetCore.Http;
using MiniETrade.Application.Common.Abstractions;
using MiniETrade.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get{
                string? userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null) return Guid.Empty;
                return Guid.Parse(userId);
            }
        }

        public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue("username");

        public string? UserMail => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    }
}
