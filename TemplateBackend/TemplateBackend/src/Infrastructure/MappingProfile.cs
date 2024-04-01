using AutoMapper;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.Register;
using TemplateBackend.Application.Features.ApplicationUsers.Queries;
using TemplateBackend.Application.Features.ApplicationUsers.Queries.GetByEmail;
using TemplateBackend.Domain.Entities;

namespace TemplateBackend.Infrastructure;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegistrationDto, ApplicationUser>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));


        CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();

        CreateMap<ApplicationUser, GetByEmailResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
            .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture));

    }
}
