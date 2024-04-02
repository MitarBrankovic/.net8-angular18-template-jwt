using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Application.Features.ApplicationUsers.Queries;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.UpdateUser;
public class UpdateUserDto: ApplicationUserDto
{
    public string Password { get; set; }
}
