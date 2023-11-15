using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniETrade.Application.Common.Abstractions.Identity;
using MiniETrade.Application.Common.Abstractions.Security;
using MiniETrade.Application.DTOs;
using MiniETrade.Domain.Entities.Identity;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MiniETrade.Infrastructure.Services.Security;

public class TokenHelper : ITokenHelper
{
    private readonly IConfiguration Configuration;
    private readonly TokenOptions _tokenOptions;
    private readonly IIdentityService _identityService;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public TokenHelper(IConfiguration configuration, IIdentityService identityService, JwtSecurityTokenHandler jwtSecurityTokenHandler)
    {
        Configuration = configuration;
        _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        _identityService = identityService;
        _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
    }

    public async Task<AppToken> CreateAppToken(AppUser user, IList<string> userRoles)
    {
        var jwtToken = CreateJwtSecurityToken(user, userRoles);
        var refreshToken = CreateRefreshToken();

        await UpdateUserRefreshToken(user, refreshToken);

        return new AppToken
        {
            JwtToken = _jwtSecurityTokenHandler.WriteToken(jwtToken),
            JwtTokenExpiration = jwtToken.ValidTo,
            RefreshToken = refreshToken
        };
    }

    public async Task<AppToken> RefreshAppToken(AppUser user, IList<string> userRoles)
    {
        var jwtToken = CreateJwtSecurityToken(user, userRoles);
        var refreshToken = CreateRefreshToken();

        user.RefreshToken = refreshToken;
        await _identityService.UpdateUser(user);

        return new AppToken
        {
            JwtToken = _jwtSecurityTokenHandler.WriteToken(jwtToken),
            JwtTokenExpiration = jwtToken.ValidTo,
            RefreshToken = refreshToken
        };
    }

    public string CreateRefreshToken()
    {
        byte[] randomNumber = new byte[64];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public JwtSecurityToken CreateJwtSecurityToken(AppUser user, IList<string> userRoles)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var jwt = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            expires: DateTime.Now.AddDays(_tokenOptions.JwtTokenExpiration),
            notBefore: DateTime.Now,
            claims: SetClaims(user, userRoles.ToList()),
            signingCredentials: signingCredentials
        );
        return jwt;
    }

    public async Task UpdateUserRefreshToken(AppUser user, string refreshToken)
    {
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_tokenOptions.RefreshTokenExpiration);

        await _identityService.UpdateUser(user);
    }

    private static IEnumerable<Claim> SetClaims(AppUser user, List<string> userRoles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("username", user.UserName),
            new Claim(ClaimTypes.Email, user.UserName)
        };
        userRoles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    public JwtSecurityToken GetTokenInfoFromExpiredToken(string jwtToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey)),
            ValidateLifetime = false
        };

        _jwtSecurityTokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase)) throw new BusinessException(AppMessages.UnauthorizedAttempt);
        return jwtSecurityToken; 
    }
    public string GetUsernameFromJwtToken(JwtSecurityToken jwtToken)
    {
        var userNameClaim = jwtToken.Claims.Where(c => c.Type == "username").FirstOrDefault()
            ?? throw new BusinessException(AppMessages.UnauthorizedAttempt);
        return userNameClaim.Value;
    }

    public IList<string> GetRolesFromJwtToken(JwtSecurityToken jwtToken)
    {
        var roles = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        return roles;
    }
}