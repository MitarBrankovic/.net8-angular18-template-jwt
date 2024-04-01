using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Application.Common.Interfaces;
using TemplateBackend.Application.Common.Models;
using TemplateBackend.Application.Contracts;
using TemplateBackend.Domain.Entities;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.GoogleLogin;

public class GoogleLoginRequest : IRequest<Result>
{
    public string Email { get; set; }
    public string Provider { get; set; }
    public string IdToken { get; set; }
}

public class GoogleLoginCommand: IRequestHandler<GoogleLoginRequest, Result>
{
    private readonly IApplicationUsersRepository _applicationUsersRepository;
    private readonly IJWTManagerRepository _jWTManagerRepository;
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _googleSettings;

    public GoogleLoginCommand(
        IApplicationUsersRepository applicationUsersRepository,
        IJWTManagerRepository jWTManagerRepository,
        IConfiguration configuration )
    {
        _applicationUsersRepository = applicationUsersRepository;
        _jWTManagerRepository = jWTManagerRepository;
        _configuration = configuration;
        _googleSettings = _configuration.GetSection("GoogleAuthSettings");
    }

    public async Task<Result> Handle(GoogleLoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var payload = await VerifyGoogleToken(request.IdToken);

            if (payload == null)
            {
                return Result.Failure(["Neuspešna autentifikacija sa Google tokenom."]);
            }
     
            var user = await _applicationUsersRepository.GetUser(payload.Email);
            if (user == null)
            {
                return Result.Failure(["Korisnik sa ovim email-om ne postoji."]);
            }
            else
            {
                var token = _jWTManagerRepository.GenerateToken(user.Email);
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
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() 
                { 
                    _googleSettings.GetSection("androidClientId").Value,
                    _googleSettings.GetSection("iosClientId").Value,
                    _googleSettings.GetSection("webClientId").Value
                }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            return payload;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
