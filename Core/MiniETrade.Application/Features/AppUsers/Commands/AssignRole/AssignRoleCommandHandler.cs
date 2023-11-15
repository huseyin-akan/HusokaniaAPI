using MediatR;
using MiniETrade.Application.BusinessRules.AppUsers;
using MiniETrade.Application.Common.Abstractions.Identity;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.AssignRole;

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