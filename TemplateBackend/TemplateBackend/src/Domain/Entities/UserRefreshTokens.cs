using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBackend.Domain.Entities;
public class UserRefreshTokens
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string RefreshToken { get; set; }
    public bool IsActive { get; set; } = true;
}
