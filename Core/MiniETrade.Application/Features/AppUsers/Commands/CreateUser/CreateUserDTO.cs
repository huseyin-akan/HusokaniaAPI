using MediatR;
using MiniETrade.Application.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.CreateUser;

public record CreateUserCommand : IRequest<CreateUserResponse>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
}

public record CreateUserResponse(string Message) : IRequestResponse;
