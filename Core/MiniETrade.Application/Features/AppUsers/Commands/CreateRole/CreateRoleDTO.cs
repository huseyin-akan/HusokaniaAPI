using MediatR;
using MiniETrade.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.CreateRole;

public record CreateRoleCommand(string Role) : IRequest<CreateRoleResponse>;
public record CreateRoleResponse();