using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBackend.Application.Common.Interfaces;
public interface IConnectionStringParser
{
    string BuildBaseUri();
}
