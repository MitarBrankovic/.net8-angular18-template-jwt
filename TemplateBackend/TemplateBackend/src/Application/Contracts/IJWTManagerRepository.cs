using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Domain.Entities;

namespace TemplateBackend.Application.Contracts;
public interface IJWTManagerRepository
{
    Tokens GenerateToken(string userName);
    Tokens GenerateRefreshToken(string userName);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
