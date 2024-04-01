using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBackend.Infrastructure.BlobSettings;
public class ParsedConnectionString
{
    public string DefaultEndpointsProtocol { get; set; } = null!;
    public string AccountName { get; set; } = null!;
    public string AccountKey { get; set; } = null!;
    public string EndpointSuffix { get; set; } = null!;
}
