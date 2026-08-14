using System.Linq.Expressions;

namespace ECommerce.Domain.Common.Specifications;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(
            left.Parameters[0],
            parameter);

        var leftExpression = leftVisitor.Visit(left.Body)!;

        var rightVisitor = new ReplaceExpressionVisitor(
            right.Parameters[0],
            parameter);

        var rightExpression = rightVisitor.Visit(right.Body)!;

        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(leftExpression, rightExpression),
            parameter);
    }
}