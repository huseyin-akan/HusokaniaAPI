using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniETrade.Application.Common.Abstractions.Identity;
using MiniETrade.Domain.Entities.Identity;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<AppUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly RoleManager<AppRole> _roleManager;

    public IdentityService(
        UserManager<AppUser> userManager,
        IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService, 
        RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _roleManager = roleManager;
    }

    public async Task<string> GetUserNameAsync(Guid userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
        return user.UserName;
    }

    public async Task<AppUser> CreateUserAsync(AppUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded) throw new UserCreateFailedException(result.Errors.First().Description);
        return user; 
    }

    public async Task<bool> IsInRoleAsync(Guid userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public Task<IdentityResult> DeleteUserAsync(Guid userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId)
            ?? throw new BusinessException(AppMessages.UserNotFound);

        return DeleteUserAsync(user);
    }

    public Task<IdentityResult> DeleteUserAsync(AppUser user)
    {
        user.Status = false;
        return _userManager.UpdateAsync(user);
    }

    public async Task<AppUser> FindByUserNameAsync(string userName)
    {
        var result = await _userManager.FindByNameAsync(userName);
        return result;
    }

    public async Task<AppUser> FindByEmailAsync(string email)
    {
        var result = await _userManager.FindByEmailAsync(email);
        return result;
    }

    public async Task<bool> CheckPasswordAsync(AppUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IList<string>> GetRolesAsync(AppUser user)
    {
        var result = await _userManager.GetRolesAsync(user);
        return result;
    }

    public async Task<bool> AddUserToRole(Guid userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString() ) ?? throw new BusinessException(AppMessages.UserNotFound);
        var roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists) throw new BusinessException(AppMessages.RoleDoesntExist);

        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded;
    }

    public async Task<bool> CreateRole(string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (roleExists) throw new BusinessException(AppMessages.RoleAlreadyExists);

        AppRole roleToCreate = new(roleName);
        var result = await _roleManager.CreateAsync(roleToCreate);
        return result.Succeeded;
    }

    public List<AppUser> GetAllUsers()
    {
        return _userManager.Users.Where(u => u.Status).ToList();
    }

    public Task<AppUser?> GetUserByIdAsync(Guid userId)
    {
        return _userManager.Users.Where(u => u.Status && u.Id == userId).FirstOrDefaultAsync();
    }

    public async Task UpdatePassword(Guid userId, string newPassword)
    {
        var userToUpdate = await GetUserByIdAsync(userId);
        var token = await _userManager.GeneratePasswordResetTokenAsync(userToUpdate ?? throw new BusinessException(AppMessages.UserNotFound));
        await _userManager.ResetPasswordAsync(userToUpdate, token, newPassword);
    }

    public async Task UpdateUser(AppUser user)
    {
        await _userManager.UpdateAsync(user);
    }
}