using ECommerce.Application.Common.Identity;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Orders.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.CustomersSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Orders.Queries.GetMyOrders;

public sealed class GetMyOrdersHandler(
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IOrderQueryService orderQueryService)
    : IRequestHandler<
        GetMyOrdersQuery,
        Result<PaginatedResult<GetMyOrdersResponse>>>
    {
    public async Task<Result<PaginatedResult<GetMyOrdersResponse>>> Handle(
        GetMyOrdersQuery request,
        CancellationToken ct)
        {
        var validationResult =
            PaginationValidator.Validate(
                request.PaginationRequest);

        if (validationResult.IsFailure)
            {
            return Result<PaginatedResult<GetMyOrdersResponse>>
                .Failure(validationResult.Errors);
            }

        var customerRepository =
            unitOfWork.Repository<Customer>();

        var customer =
            await customerRepository.FirstOrDefaultAsync(
                new CustomerByApplicationUserIdSpecification(
                    currentUser.UserId),
                ct);

        if (customer is null || customer.IsDeleted)
            {
            return Result<PaginatedResult<GetMyOrdersResponse>>
                .Failure(
                    OrderErrors.CustomerUnavailable);
            }

        return await orderQueryService.GetMyOrders(
            customer.Id,
            request.PaginationRequest,
            ct);
        }
    }