using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Application.Common.Models;
using TemplateBackend.Application.Contracts;
using TemplateBackend.Application.Features.ApplicationUsers.Queries;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.UpdateUser;
public class UpdateUserRequest: IRequest<Result>
{
    public UpdateUserDto UpdateUserDto { get; set; }
}

public class UpdateUserHandler: IRequestHandler<UpdateUserRequest, Result>
{
    private readonly IApplicationUsersRepository _applicationUsersRepository;

    public UpdateUserHandler(IApplicationUsersRepository applicationUsersRepository)
    {
        _applicationUsersRepository = applicationUsersRepository;
    }

    public async Task<Result> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _applicationUsersRepository.GetUser(request.UpdateUserDto.Email);
            user.FullName = request.UpdateUserDto.FullName;
            user.DateOfBirth = request.UpdateUserDto.DateOfBirth;
            user.ProfilePicture = request.UpdateUserDto.ProfilePicture;
            user.Email = request.UpdateUserDto.Email;
            user.Password = request.UpdateUserDto.Password;

            var updatedUser = await _applicationUsersRepository.UpdateUser(user);
            return Result.Success(updatedUser);
        }
        catch (Exception ex)
        {
            return Result.Failure([ex.Message]);
        }

    }
}
