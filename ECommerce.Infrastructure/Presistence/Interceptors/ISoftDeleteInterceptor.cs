

using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Presistence.Interceptors;

public interface ISoftDeleteInterceptor
{
    void Apply(DbContext db);
}
