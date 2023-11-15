using MediatR;
using Application.BusinessRules.AppUsers;
using Application.Common.Abstractions.Identity;
using Domain.Exceptions;
using Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.AssignRole;

public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, AssignRoleResponse>
{
    private readonly IIdentityService _identityService;
    private readonly AppUserBusinessRules _appUserBusinessRules;

    public AssignRoleCommandHandler(IIdentityService identityService, AppUserBusinessRules appUserBusinessRules)
    {
        _identityService = identityService;
        _appUserBusinessRules = appUserBusinessRules;
    }

    public async Task<AssignRoleResponse> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.AddUserToRole(request.UserId, request.Role);
        if (!result) throw new BusinessException(AppMessages.UnexpectedError);

        return new(Message: "Role is assigned to the user"); //TODO-HUS magis string.
    }
}