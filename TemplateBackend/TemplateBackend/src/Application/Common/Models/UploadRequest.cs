using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBackend.Application.Common.Models;
public record UploadRequest(Stream File, string FileName, string ContentType);
