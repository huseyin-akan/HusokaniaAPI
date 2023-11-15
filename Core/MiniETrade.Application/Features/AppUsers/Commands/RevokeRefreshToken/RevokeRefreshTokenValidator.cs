using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.RevokeRefreshToken;

public class RevokeRefreshTokenValidator : AbstractValidator<RevokeRefreshTokenCommand>
{
    public RevokeRefreshTokenValidator()
    {
        
    }
}
