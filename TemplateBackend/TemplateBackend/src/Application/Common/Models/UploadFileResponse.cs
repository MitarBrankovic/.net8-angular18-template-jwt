using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBackend.Application.Common.Models;
    public class UploadFileResponse
{
    public string Name { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public string? Uri { get; set; }
}
