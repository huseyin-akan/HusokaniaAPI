using MediatR;
using Application.Common.Abstractions.Identity;
using Application.Common.Abstractions.Security;
using Application.Features.AppUsers.Commands.RefreshToken;
using Domain.Exceptions;
using Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.RevokeRefreshToken;

public class RevokeRefreshTokenHandler : IRequestHandler<RevokeRefreshTokenCommand, RevokeRefreshTokenResponse>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenHelper _tokenHelper;

    public RevokeRefreshTokenHandler(IIdentityService identityService, ITokenHelper tokenHelper)
    {
        _identityService = identityService;
        _tokenHelper = tokenHelper;
    }

    public async Task<RevokeRefreshTokenResponse> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByUserNameAsync(request.UserName) ?? throw new UnAuthorizedException(AppMessages.UnauthorizedAttempt);

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _identityService.UpdateUser(user);

        return new();
    }
}