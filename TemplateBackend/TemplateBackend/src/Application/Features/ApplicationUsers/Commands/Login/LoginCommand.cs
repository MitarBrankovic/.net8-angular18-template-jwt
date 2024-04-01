using TemplateBackend.Application.Common.Models;
using TemplateBackend.Application.Contracts;
using TemplateBackend.Domain.Entities;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.Login;

public class LoginRequest : IRequest<Result>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}

public class LoginCommand : IRequestHandler<LoginRequest, Result>
{
    private readonly IApplicationUsersRepository _applicationUsersRepository;
    private readonly IJWTManagerRepository _jwtManagerRepository;


    public LoginCommand(
        IApplicationUsersRepository applicationUsersRepository,
        IJWTManagerRepository jwtManagerRepository)
    {
        _applicationUsersRepository = applicationUsersRepository;
        _jwtManagerRepository = jwtManagerRepository;
    }

    public async Task<Result> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _applicationUsersRepository.GetUser(request.Username);

            if (user == null)
            {
                return Result.Failure(["Korisnik sa ovim email-om ne postoji."]);
            }
            
            var userWithCredentialsExists = await _applicationUsersRepository.IsValidUserAsync(request.Username, request.Password);

            if (!userWithCredentialsExists) return Result.Failure(["Pogrešni kredencijali."]);

            var token = _jwtManagerRepository.GenerateToken(request.Username);
            if (token == null)
            {
                return Result.Failure(["Neuspešno generisanje tokena."]);
            }

            UserRefreshTokens userRefreshToken = new()
            {
                RefreshToken = token.RefreshToken,
                Email = user.Email
            };
            _applicationUsersRepository.AddUserRefreshTokens(userRefreshToken);

            return Result.Success(token);         
        }
        catch (Exception)
        {
            throw;
        }

    }

}
