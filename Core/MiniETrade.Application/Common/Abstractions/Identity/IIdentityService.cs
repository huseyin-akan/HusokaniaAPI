using Microsoft.AspNetCore.Identity;
using MiniETrade.Application.DTOs;
using MiniETrade.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Identity;

public interface IIdentityService
{
    Task<bool> AuthorizeAsync(Guid userId, string policyName);
    Task<bool> AddUserToRole(Guid userId, string role);
    Task<bool> CheckPasswordAsync(AppUser user, string password);
    Task<bool> CreateRole(string roleName);
    Task<AppUser> CreateUserAsync(AppUser user, string password);
    Task<IdentityResult> DeleteUserAsync(Guid userId);
    Task<IdentityResult> DeleteUserAsync(AppUser user);
    Task<AppUser> FindByEmailAsync(string email);
    Task<AppUser> FindByUserNameAsync(string userName);
    List<AppUser> GetAllUsers();
    Task<IList<string>> GetRolesAsync(AppUser user);
    Task<string> GetUserNameAsync(Guid userId);
    Task<AppUser?> GetUserByIdAsync(Guid userId);
    Task<bool> IsInRoleAsync(Guid userId, string role);
    Task UpdatePassword(Guid userId, string newPassword);
    Task UpdateUser(AppUser user);
}