using MediatR;
using Application.Common.Abstractions;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.LoginUser;

public record LoginUserCommand(string UsernameOrEmail, string Password) : IRequest<LoginUserResponse>;
public record LoginUserResponse(AppToken Token) :IRequestResponse;