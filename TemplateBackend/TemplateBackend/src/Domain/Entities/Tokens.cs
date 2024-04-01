using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBackend.Domain.Entities;
public class Tokens
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
