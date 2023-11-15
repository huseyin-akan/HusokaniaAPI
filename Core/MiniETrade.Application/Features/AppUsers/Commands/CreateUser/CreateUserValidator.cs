using FluentValidation;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.CreateUser;

public class CreateAppUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateAppUserValidator()
    {
        RuleFor(u => u.Password)
            .NotEmpty().WithMessage(AppMessages.PasswordEmpty)
            .Must(p => p?.Trim().Length >= 6).WithMessage(AppMessages.PasswordMinLength);
            //.MinimumLength(6).WithMessage(AppMessages.PasswordMinLength);

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage(AppMessages.InvalidEmailAddress)
            .EmailAddress().WithMessage(AppMessages.InvalidEmailAddress);

        //RuleFor(u => u.)
        //    .Must(s => s >= 0)
        //    .WithMessage("Stock bilgisi 0'dan küçük olamaz!");
    }
}