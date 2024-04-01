using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Application.Features.ApplicationUsers.Commands;

namespace TemplateBackend.Application.Features.ApplicationUsers.Queries.GetByEmail;
public class GetByEmailResponseDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }

    public DateOnly DateOfBirth { get; set; }
    public string? ProfilePicture { get; set; }

}
