using TemplateBackend.Application.Features.ApplicationUsers.Commands;

namespace TemplateBackend.Application.Features.ApplicationUsers.Queries;
public class ApplicationUserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }

    public DateOnly DateOfBirth { get; set; }
    public string? ProfilePicture { get; set; }
}
