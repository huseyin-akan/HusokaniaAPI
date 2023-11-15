using MediatR;
using Application.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.AssignRole;

public record AssignRoleCommand(Guid UserId, string Role) : IRequest<AssignRoleResponse>;

public record AssignRoleResponse(string Message) : IRequestResponse;
