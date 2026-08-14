using ECommerce.Domain.Common.Specifications;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Presistence.Specification
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>
            (
            IQueryable<T> inputQuery,
            ISpecification<T> specification)
            where T : class
        {
            IQueryable<T> query = inputQuery;

            // Apply filtering
            if (specification.Criteria is not null)
            {
                query = query.Where(specification.Criteria);
            }

            // Apply sorting
            if (specification.OrderBy is not null)
            {
                query = query.OrderBy(specification.OrderBy);
            }

            if (specification.OrderByDescending is not null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            // Apply includes
            query = specification.Includes.Aggregate(
                query,
                (current, include) => current.Include(include));

            // Apply pagination
            if (specification.IsPagingEnabled)
            {
                query = query
                    .Skip(specification.Skip)
                    .Take(specification.Take);
            }

            return query;
        }
    }
}