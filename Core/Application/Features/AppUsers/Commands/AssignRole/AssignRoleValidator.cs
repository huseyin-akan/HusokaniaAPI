using FluentValidation;
using Application.Features.AppUsers.Commands.CreateUser;
using Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.AssignRole;

public class AssignRoleValidator : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleValidator()
    {

    }
}