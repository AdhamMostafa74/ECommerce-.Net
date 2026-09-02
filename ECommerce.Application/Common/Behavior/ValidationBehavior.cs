using ECommerce.Application.Common.Results;
using ECommerce.Domain.Common.Types;
using ECommerce.Domain.Common;
using FluentValidation;
using MediatR;

namespace ECommerce.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    IResultFactory resultFactory)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
    private readonly IResultFactory _resultFactory = resultFactory;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(
                validator => validator.ValidateAsync(
                    context,
                    cancellationToken)));

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .ToList();

        if (failures.Count == 0)
            return await next();

        var errors = failures
            .Select(failure => new Error(
                failure.ErrorCode,
                failure.ErrorMessage,
                ErrorType.Validation))
            .ToList();

        return (TResponse)_resultFactory.CreateFailure(
            typeof(TResponse),
            errors);
    }
}