using System.ComponentModel.DataAnnotations;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.Register;
public class RegistrationDto
{
    public string FullName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //public string ConfirmPassword { get; set; }

    public DateOnly DateOfBirth { get; set; }
    public string? ProfilePicture { get; set; }
    
}

