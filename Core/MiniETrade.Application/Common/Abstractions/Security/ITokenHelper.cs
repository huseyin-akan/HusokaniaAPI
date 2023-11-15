using MiniETrade.Domain.Entities.Identity;
using MiniETrade.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace MiniETrade.Application.Common.Abstractions.Security;

public interface ITokenHelper
{
    Task<AppToken> CreateAppToken(AppUser user, IList<string> userRoles);
    JwtSecurityToken CreateJwtSecurityToken(AppUser user, IList<string> userRoles);
    string CreateRefreshToken();
    Task<AppToken> RefreshAppToken(AppUser user, IList<string> userRoles);
    string GetUsernameFromJwtToken(JwtSecurityToken jwtToken);
    IList<string> GetRolesFromJwtToken(JwtSecurityToken jwtToken);
    JwtSecurityToken GetTokenInfoFromExpiredToken(string jwtToken);
    Task UpdateUserRefreshToken(AppUser user, string refreshToken);
}