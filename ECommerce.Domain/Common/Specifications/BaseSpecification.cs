using System.Linq.Expressions;

namespace ECommerce.Domain.Common.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    protected BaseSpecification()
    {
    }

    public Expression<Func<T, bool>>? Criteria { get; protected set; }

    public List<Expression<Func<T, object>>> Includes { get; } = [];

    public Expression<Func<T, object>>? OrderBy { get; protected set; }

    public Expression<Func<T, object>>? OrderByDescending { get; protected set; }

    public int Skip { get; protected set; }

    public int Take { get; protected set; }

    public bool IsPagingEnabled { get; protected set; }

    protected void AddCriteria(Expression<Func<T, bool>> criteria)
    {
        if (Criteria is null)
        {
            Criteria = criteria;
            return;
        }

        Criteria = Criteria.And(criteria);
    }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}