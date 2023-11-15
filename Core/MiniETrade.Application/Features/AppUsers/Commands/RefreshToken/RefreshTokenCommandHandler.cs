using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniETrade.Application.Common.Abstractions.Identity;
using MiniETrade.Application.Common.Abstractions.Security;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenHelper _tokenHelper;

    public RefreshTokenCommandHandler(IIdentityService identityService, ITokenHelper tokenHelper)
    {
        _identityService = identityService;
        _tokenHelper = tokenHelper;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var jwtToken = _tokenHelper.GetTokenInfoFromExpiredToken(request.AccessToken) 
            ?? throw new UnAuthorizedException(AppMessages.UnauthorizedAttempt);
        var userName = _tokenHelper.GetUsernameFromJwtToken(jwtToken) ?? throw new UnAuthorizedException(AppMessages.UnauthorizedAttempt);

        var user = await _identityService.FindByUserNameAsync(userName); 

        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new UnAuthorizedException(AppMessages.UnauthorizedAttempt);

        var userRoles = _tokenHelper.GetRolesFromJwtToken(jwtToken) ?? throw new UnAuthorizedException(AppMessages.UnauthorizedAttempt);
        var newAccessToken = await _tokenHelper.RefreshAppToken(user, userRoles);

        return new(newAccessToken);
    }
}