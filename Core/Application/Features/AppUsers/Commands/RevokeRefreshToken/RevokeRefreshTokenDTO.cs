using MediatR;
using Application.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.RevokeRefreshToken;

public record RevokeRefreshTokenCommand(string UserName) : IRequest<RevokeRefreshTokenResponse>;
public record RevokeRefreshTokenResponse() : IRequestResponse;