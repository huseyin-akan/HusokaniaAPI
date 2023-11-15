using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniETrade.Application.Features.AppUsers.Commands.AssignRole;
using MiniETrade.Application.Features.AppUsers.Commands.CreateRole;
using MiniETrade.Application.Features.AppUsers.Commands.CreateUser;
using MiniETrade.Application.Features.AppUsers.Commands.LoginUser;
using MiniETrade.Application.Features.AppUsers.Commands.RefreshToken;
using MiniETrade.Application.Features.AppUsers.Commands.RevokeRefreshToken;

namespace MiniETrade.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("createuser")]
    public async Task<IActionResult> CreateUser(CreateUserCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("refreshtoken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("revokerefreshtoken")]
    public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("createrole")]
    public async Task<IActionResult> CreateRole(CreateRoleCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("assignrole")]
    public async Task<IActionResult> AssignRole(AssignRoleCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}