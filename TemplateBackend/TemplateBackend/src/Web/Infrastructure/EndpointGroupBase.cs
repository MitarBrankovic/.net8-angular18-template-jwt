using Microsoft.AspNetCore.Mvc;

namespace TemplateBackend.Web.Infrastructure;

public abstract class EndpointGroupBase
{
    public abstract void Map(WebApplication app);
}
