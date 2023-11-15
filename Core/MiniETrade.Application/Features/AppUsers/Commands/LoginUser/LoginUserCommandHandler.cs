using MediatR;
using MiniETrade.Application.Common.Abstractions.Identity;
using MiniETrade.Application.Common.Abstractions.Security;
using MiniETrade.Domain.Entities.Identity;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenHelper _tokenHelper;

    public LoginUserCommandHandler(IIdentityService identityService, ITokenHelper tokenHelper)
    {
        _identityService = identityService;
        _tokenHelper = tokenHelper;
    }

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await GetUserOrThrowError(request);

        await CheckIfPasswordMatches(user, request.Password);

        var userRoles = await _identityService.GetRolesAsync(user);
        var accessToken = await _tokenHelper.CreateAppToken(user, userRoles);

        return new(accessToken);
    }

    private async Task<AppUser> GetUserOrThrowError(LoginUserCommand request)
    {
        AppUser? user = await _identityService.FindByUserNameAsync(request.UsernameOrEmail);
        user ??= await _identityService.FindByEmailAsync(request.UsernameOrEmail);
        if (user is null) throw new UnAuthorizedException(AppMessages.LoginUnsuccessful);
        return user;
    }

    private async Task CheckIfPasswordMatches(AppUser user, string password)
    {
        if (await _identityService.CheckPasswordAsync(user, password) != true)
            throw new UnAuthorizedException(AppMessages.LoginUnsuccessful);
    }
}