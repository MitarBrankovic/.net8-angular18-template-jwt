using TemplateBackend.Application.Features.ApplicationUsers.Commands.DeleteUser;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.FacebookLogin;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.GoogleLogin;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.Login;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.RefreshToken;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.Register;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.Registration;
using TemplateBackend.Application.Features.ApplicationUsers.Queries.GetByEmail;
using TemplateBackend.Domain.Entities;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace TemplateBackend.Web.Endpoints;

public class ApplicationUsers : EndpointGroupBase
{

    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(LoginUser, "LoginUser")
            .MapPost(RefreshToken, "RefreshToken")
            .MapPost(GoogleLogin, "GoogleLogin")
            .MapPost(FacebookLogin, "FacebookLogin/{accessToken}")
            .MapPost(RegisterUser, "RegisterUser")
            .MapDelete(DeleteUser, "DeleteUser/{email}")
            .MapGet(GetByEmail, "GetByEmail/{email}");

    }

    [AllowAnonymous]
    public async Task<IResult> LoginUser([FromBody] LoginDto dto, ISender sender)
    {
        var loginRequest = new LoginRequest
        {
            Username = dto.Username,
            Password = dto.Password,
            RememberMe = dto.RememberMe
        };
        var response = await sender.Send(loginRequest);
        return response.Succeeded ? Results.Ok(response.Data) : Results.BadRequest(response.Errors);
    }
    
    public async Task<IResult> RefreshToken([FromBody] Tokens token, ISender sender)
    {
        var refreshTokenRequest = new RefreshTokenRequest
        {
            OldToken = token
        };
        var response = await sender.Send(refreshTokenRequest);
        return response.Succeeded ? Results.Ok(response.Data) : Results.Unauthorized();
    }

    [AllowAnonymous]
    public async Task<IResult> GoogleLogin([FromBody] GoogleLoginDto dto, ISender sender)
    {
        var loginRequest = new GoogleLoginRequest
        {
            Email = dto.Email,
            Provider = dto.Provider,
            IdToken = dto.IdToken
        };
        var response = await sender.Send(loginRequest);
        return response.Succeeded ? Results.Ok(response.Data) : Results.BadRequest("Login failed.");
    }

    [AllowAnonymous]
    public async Task<IResult> FacebookLogin(string accessToken, ISender sender)
    {
        var loginRequest = new FacebookLoginRequest
        {
            AccessToken = accessToken
        };
        var response = await sender.Send(loginRequest);
        return response.Succeeded ? Results.Ok(response.Data) : Results.BadRequest("Login failed.");
    }

    [AllowAnonymous]
    public async Task<IResult> RegisterUser([FromBody] RegistrationDto registrationDto, ISender sender)
    {
        var request = new RegistrationRequest
        {
            RegistrationDto = registrationDto
        };

        var response = await sender.Send(request);
        return response.Succeeded ? Results.Ok(response.Data) : Results.BadRequest(response);
    }


    public async Task<IResult> DeleteUser(string email, ISender sender)
    {
        var request = new DeleteUserRequest
        {
            Email = email
        };
        var response = await sender.Send(request);
        return response.Succeeded ? Results.Ok(response.Data) : Results.BadRequest(response);
    }

    public async Task<IResult> GetByEmail(string email, ISender sender)
    {
        var request = new GetByEmailRequest
        {
            Email = email
        };
        var response = await sender.Send(request);
        return response.Succeeded ? Results.Ok(response.Data) : Results.BadRequest(response.Errors);
    }
}
