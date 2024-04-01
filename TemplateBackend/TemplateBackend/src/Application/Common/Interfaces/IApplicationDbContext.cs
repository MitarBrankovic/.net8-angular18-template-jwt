using TemplateBackend.Domain.Entities;

namespace TemplateBackend.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
