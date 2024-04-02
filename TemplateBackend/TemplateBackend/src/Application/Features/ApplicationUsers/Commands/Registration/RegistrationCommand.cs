using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bebac.Application.Common.Interfaces;
using TemplateBackend.Application.Common.Exceptions;
using TemplateBackend.Application.Common.Interfaces;
using TemplateBackend.Application.Common.Models;
using TemplateBackend.Application.Contracts;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.Register;
using TemplateBackend.Domain.Entities;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.Registration;

public class RegistrationRequest : IRequest<Result>
{
    public RegistrationDto RegistrationDto { get; set; }
}

public class RegistrationCommand : IRequestHandler<RegistrationRequest, Result>
{
    private readonly IApplicationUsersRepository _applicationUsersRepository;
    //private readonly IBlobService _blobService;
    private readonly IMapper _mapper;

    public RegistrationCommand(
        IApplicationUsersRepository applicationUsersRepository,
        //IBlobService blobService,
        IMapper mapper
        )
    {
        _applicationUsersRepository = applicationUsersRepository;
        //_blobService = blobService;
        _mapper = mapper;
    }

    public async Task<Result> Handle(RegistrationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await _applicationUsersRepository.GetUser(request.RegistrationDto.Email);

            if (existingUser != null)
            {
                return Result.Failure(["User with this email already exists."]);
            }
            
            var newUser = _mapper.Map<ApplicationUser>(request.RegistrationDto);
            //newUser.PictureFolderName = Guid.NewGuid();

            //if (!string.IsNullOrEmpty(request.RegistrationDto?.ProfilePicture))
            //{
            //    Guid filename = Guid.NewGuid();
            //    newUser.ProfilePicture = filename.ToString();
            //    await _blobService.UploadProfilePictureAsync(request.RegistrationDto.ProfilePicture, newUser.ProfilePicture, newUser.PictureFolderName.ToString());
            //}

            var result = await _applicationUsersRepository.CreateUser(newUser);
            return result != Guid.Empty
                ? Result.Success(result)
                : Result.Failure(["Failed to register user."]);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }       
    }
}
