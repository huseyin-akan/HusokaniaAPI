using MediatR;
using MiniETrade.Application.Common.Abstractions;
using MiniETrade.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.LoginUser;

public record LoginUserCommand(string UsernameOrEmail, string Password) : IRequest<LoginUserResponse>;
public record LoginUserResponse(AppToken Token) :IRequestResponse;