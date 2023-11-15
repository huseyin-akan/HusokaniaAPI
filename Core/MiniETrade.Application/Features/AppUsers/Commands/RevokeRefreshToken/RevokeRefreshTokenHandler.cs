using MediatR;
using MiniETrade.Application.Common.Abstractions.Identity;
using MiniETrade.Application.Common.Abstractions.Security;
using MiniETrade.Application.Features.AppUsers.Commands.RefreshToken;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.RevokeRefreshToken;

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