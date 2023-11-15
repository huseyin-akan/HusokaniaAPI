using FluentValidation;
using Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.RefreshToken;

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