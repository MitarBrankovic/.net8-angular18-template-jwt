
namespace TemplateBackend.Domain.Entities;
public class ApplicationUser
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string? ProfilePicture { get; set; }
    public Guid PictureFolderName { get; set; }
}
