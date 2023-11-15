using MediatR;
using Application.Common.Abstractions;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.RefreshToken;

public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<RefreshTokenResponse>;

public record RefreshTokenResponse(AppToken Token) :IRequestResponse;