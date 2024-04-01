using TemplateBackend.Application.Common.Models;
using TemplateBackend.Application.Contracts;
using TemplateBackend.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using Newtonsoft.Json;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.FacebookLogin;

public class FacebookLoginRequest : IRequest<Result>
{
    public string AccessToken { get; set; }
    public string Provider { get; set; }
}

public class FacebookLoginCommand: IRequestHandler<FacebookLoginRequest, Result>
{
    private readonly IApplicationUsersRepository _applicationUsersRepository;
    private readonly IJWTManagerRepository _jWTManagerRepository;
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _facebookSettings;
    private readonly HttpClient _httpClient;

    public FacebookLoginCommand(
        IApplicationUsersRepository applicationUsersRepository,
        IJWTManagerRepository jWTManagerRepository,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _applicationUsersRepository = applicationUsersRepository;
        _jWTManagerRepository = jWTManagerRepository;
        _configuration = configuration;
        _facebookSettings = _configuration.GetSection("FacebookAuthSettings");
        _httpClient = httpClientFactory.CreateClient("Facebook");
    }

    public async Task<Result> Handle(FacebookLoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var userInfoUrl = _facebookSettings.GetSection("UserInfoUrl").Value;
            var appId = _facebookSettings.GetSection("AppId").Value;
            var appSecret = _facebookSettings.GetSection("AppSecret").Value;
            var url = string.Format(userInfoUrl, request.AccessToken);
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Result.Failure(["Neuspešna autentifikacija preko Facebook-a."]);
            }
            
            var responseAsString = await response.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeObject<FacebookUserInfoResponse>(responseAsString);        

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
}
