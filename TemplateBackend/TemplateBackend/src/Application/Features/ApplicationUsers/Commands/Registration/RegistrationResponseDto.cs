using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.Registration;
public class RegistrationResponseDto
{
    public Guid UserId { get; set; }
    public List<string> Errors { get; set; }
}
