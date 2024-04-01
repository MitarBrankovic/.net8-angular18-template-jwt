using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBackend.Infrastructure.BlobSettings;
public class BlobStorageSettings
{
    public const string OptionName = "BlobStorage";
    public string ConnectionString { get; set; } = null!;
}
