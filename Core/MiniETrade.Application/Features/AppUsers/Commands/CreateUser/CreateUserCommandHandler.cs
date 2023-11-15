using MediatR;
using MiniETrade.Application.BusinessRules.AppUsers;
using MiniETrade.Application.Common.Abstractions.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IIdentityService _identityService;
    private readonly AppUserBusinessRules _appUserBusinessRules;

    public CreateUserCommandHandler(IIdentityService identityService, AppUserBusinessRules appUserBusinessRules)
    {
        _identityService = identityService;
        _appUserBusinessRules = appUserBusinessRules;
    }

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        AppUserBusinessRules.CheckIfPasswordMatches(request.Password, request.PasswordConfirm);
        await _appUserBusinessRules.CheckIfUsernameIsAvailable(request.Username);
        await _appUserBusinessRules.CheckIfEmailIsAvailable(request.Email);

        var createdUser = await _identityService.CreateUserAsync(new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Username,
            NormalizedUserName = request.Username.Normalize(),
            Email = request.Email,
            NormalizedEmail = request.Email.Normalize(),
            EmailConfirmed = true,
            LockoutEnabled = false,
            PhoneNumber = request.PhoneNumber,
            SecurityStamp = Guid.NewGuid().ToString(),
        }, request.Password);

        return new(Message: "User registered with userId: " + createdUser.Id);
    }
}