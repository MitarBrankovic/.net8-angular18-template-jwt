using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.GoogleLogin;
public class GoogleLoginDto
{
    public string Email { get; set; }
    public string Provider { get; set; }
    public string IdToken { get; set; }
}
