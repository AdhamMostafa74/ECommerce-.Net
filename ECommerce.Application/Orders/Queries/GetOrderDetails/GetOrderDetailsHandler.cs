using ECommerce.Application.Common.Identity;
using ECommerce.Application.Orders;
using ECommerce.Application.Orders.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.CustomersSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Orders.Queries.GetOrderDetails;

public sealed class GetOrderDetailsHandler(
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IOrderQueryService orderQueryService)
    : IRequestHandler<
        GetOrderDetailsQuery,
        Result<GetOrderDetailsResponse>>
{
    public async Task<Result<GetOrderDetailsResponse>> Handle(
        GetOrderDetailsQuery request,
        CancellationToken ct)
    {
        var customerRepository =
            unitOfWork.Repository<Customer>();

        var customer =
            await customerRepository.FirstOrDefaultAsync(
                new CustomerByApplicationUserIdSpecification(
                    currentUser.UserId),
                ct);

        if (customer is null || customer.IsDeleted)
        {
            return Result<GetOrderDetailsResponse>
                .Failure(
                    OrderErrors.CustomerUnavailable);
        }

        return await orderQueryService.GetOrderDetails(
            request.OrderId,
            customer.Id,
            ct);
    }
}