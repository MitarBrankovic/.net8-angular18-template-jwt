using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Application.Common.Models;
using TemplateBackend.Application.Contracts;
using TemplateBackend.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.RefreshToken;
public class RefreshTokenRequest: IRequest<Result>
{
    public Tokens OldToken { get; set; }
}

public class RefreshTokenHandler: IRequestHandler<RefreshTokenRequest, Result>
{
    private readonly IApplicationUsersRepository _applicationUsersRepository;
    private readonly IJWTManagerRepository _jWTManagerRepository;
    
    public RefreshTokenHandler(
        IApplicationUsersRepository applicationUsersRepository,
        IJWTManagerRepository jWTManagerRepository) 
    {
        _applicationUsersRepository = applicationUsersRepository;
        _jWTManagerRepository = jWTManagerRepository;
    }
    
    public async Task<Result> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var principal = _jWTManagerRepository.GetPrincipalFromExpiredToken(request.OldToken.AccessToken);
        var username = principal.Identity?.Name;

        var savedRefreshToken = _applicationUsersRepository.GetSavedRefreshTokens(username, request.OldToken.RefreshToken);

        if (savedRefreshToken.RefreshToken != request.OldToken.RefreshToken)
        {
            return Result.Failure(["Not valid refresh token."]);
        }

        var newJwtToken = _jWTManagerRepository.GenerateRefreshToken(username);

        if (newJwtToken == null)
        {
            return Result.Failure(["Failed to generate new token."]);
        }

        UserRefreshTokens obj = new UserRefreshTokens
        {
            RefreshToken = newJwtToken.RefreshToken,
            Email = username
        };

        await _applicationUsersRepository.DeleteUserRefreshTokens(username, request.OldToken.RefreshToken);
        _applicationUsersRepository.AddUserRefreshTokens(obj);

        return Result.Success(newJwtToken);
    }
}
