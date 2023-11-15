using MediatR;
using Application.Common.Abstractions.Identity;
using Domain.Exceptions;
using Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.CreateRole;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, CreateRoleResponse>
{
    private readonly IIdentityService _identityService;

    public CreateRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<CreateRoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var roleResult = await _identityService.CreateRole(request.Role);
        if (!roleResult) throw new BusinessException(AppMessages.UnexpectedError);
        return new();
    }
}