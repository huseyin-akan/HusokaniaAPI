using FluentValidation;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.RefreshToken;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(t => t.AccessToken)
           .NotEmpty().WithMessage(AppMessages.FieldCannotBeEmpty);

        RuleFor(t => t.RefreshToken)
           .NotEmpty().WithMessage(AppMessages.FieldCannotBeEmpty);
    }
}