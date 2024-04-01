using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Application.Common.Interfaces;
using TemplateBackend.Infrastructure.BlobSettings;

namespace TemplateBackend.Infrastructure.Services;
public class BlobSettingsParser : IConnectionStringParser
{
    private readonly BlobStorageSettings _settings;
    public BlobSettingsParser(BlobStorageSettings settings)
    {
        _settings = settings;
    }

    public string BuildBaseUri()
    {
        var parsed = ParseConnectionString();
        return $"{parsed.DefaultEndpointsProtocol}://{parsed.AccountName}.blob.{parsed.EndpointSuffix}";
    }

    private ParsedConnectionString ParseConnectionString()
    {

        var connStringArray = _settings.ConnectionString.Split(';');

        var dictionary = connStringArray
            .Select(item => item.Split('='))
            .ToDictionary(itemKeyValue => itemKeyValue[0], itemKeyValue => itemKeyValue[1]);
        return new ParsedConnectionString
        {
            DefaultEndpointsProtocol = dictionary.TryGetValue("DefaultEndpointsProtocol", out var value) ? value : "",
            AccountName = dictionary.TryGetValue("AccountName", out var name) ? name : "",
            AccountKey = dictionary.TryGetValue("AccountKey", out var key) ? key : "",
            EndpointSuffix = dictionary.TryGetValue("EndpointSuffix", out var suf) ? suf : "",
        };
    }
}
