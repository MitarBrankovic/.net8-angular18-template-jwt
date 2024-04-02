using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Application.Common.Interfaces;
using TemplateBackend.Application.Contracts;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.Login;
using TemplateBackend.Domain.Entities;
using TemplateBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TemplateBackend.Infrastructure.Repository;
public class ApplicationUsersRepository : IApplicationUsersRepository
{
    private readonly ApplicationDbContext _context;
    public ApplicationUsersRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateUser(ApplicationUser user)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
        catch (Exception)
        {
            return Guid.Empty;
        }
    }

    public async Task<ApplicationUser> UpdateUser(ApplicationUser updatedUser)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == updatedUser.Id);
        if (user != null)
        {
            user.FullName = updatedUser.FullName;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;
            user.DateOfBirth = updatedUser.DateOfBirth;
            user.ProfilePicture = updatedUser.ProfilePicture;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        return null;
    }

    public async Task<bool> DeleteUser(string email)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<List<ApplicationUser>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<ApplicationUser> GetUser(string email)
    {
        return await _context.Users.Where(u => u.Email == email)
                            .FirstOrDefaultAsync();
    }

    public async Task<ApplicationUser> GetUser(Guid id)
    {
        return await _context.Users.Where(u => u.Id == id)
                    .FirstOrDefaultAsync();
    }

    public async Task<bool> IsValidUserAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == username && u.Password == password);

        if (user != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    
    public UserRefreshTokens GetSavedRefreshTokens(string email, string refreshToken)
    {
        return _context.UserRefreshTokens.FirstOrDefault(x => x.Email == email && x.RefreshToken == refreshToken && x.IsActive == true);

    }

    public UserRefreshTokens AddUserRefreshTokens(UserRefreshTokens refreshToken)
    {
        _context.UserRefreshTokens.Add(refreshToken);
        _context.SaveChanges();
        return refreshToken;
    }
    public async Task<bool> DeleteUserRefreshTokens(string email, string refreshToken)
    {
        var item = _context.UserRefreshTokens.FirstOrDefault(x => x.Email == email && x.RefreshToken == refreshToken);
        if (item != null)
        {
            _context.UserRefreshTokens.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
