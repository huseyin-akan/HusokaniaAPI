using FluentValidation;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.LoginUser;

public class LoginUserValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserValidator()
    {
        RuleFor(u => u.UsernameOrEmail)
           .NotEmpty().WithMessage(AppMessages.UsernameOrEmailEmpty);

        RuleFor(u => u.Password)
           .NotEmpty().WithMessage(AppMessages.UsernameOrEmailEmpty)
           .Must(p => p.Trim().Length >= 6).WithMessage(AppMessages.PasswordMinLength);
    }
}