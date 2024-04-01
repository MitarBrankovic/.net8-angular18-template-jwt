using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.Login;
using TemplateBackend.Domain.Entities;

namespace TemplateBackend.Application.Contracts;
public interface IApplicationUsersRepository
{
    Task<Guid> CreateUser(ApplicationUser user);
    Task<ApplicationUser> UpdateUser(ApplicationUser updatedUser);
    Task<bool> DeleteUser(string username);
    Task<ApplicationUser> GetUser(string username);
    Task<ApplicationUser> GetUser(Guid id);
    Task<List<ApplicationUser>> GetAllUsers();
    Task<bool> IsValidUserAsync(string username, string password);

    UserRefreshTokens AddUserRefreshTokens(UserRefreshTokens user);

    UserRefreshTokens GetSavedRefreshTokens(string username, string refreshtoken);

    Task<bool> DeleteUserRefreshTokens(string username, string refreshToken);
}
