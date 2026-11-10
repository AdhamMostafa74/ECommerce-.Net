using ECommerce.Application.Account.Errors;
using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.CustomersSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Account.Queries.GetProfile;

public sealed class GetProfileHandler(
    ICurrentUser currentUser,
    IIdentityService identityService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<GetProfileQuery, Result<ProfileResponse>>
{
    public async Task<Result<ProfileResponse>> Handle(
        GetProfileQuery request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        var customerRepository = unitOfWork.Repository<Customer>();

        var customer = await customerRepository.FirstOrDefaultAsync(
            new CustomerByApplicationUserIdSpecification(userId),
            cancellationToken);

        if (customer is null || customer.IsDeleted)
        {
            return Result<ProfileResponse>.Failure(
                AccountErrors.ProfileNotFound);
        }

        var userInfo = await identityService.GetUserBasicInfoAsync(
            userId,
            cancellationToken);

        if (userInfo is null)
        {
            return Result<ProfileResponse>.Failure(
                AccountErrors.ProfileNotFound);
        }

        var response = new ProfileResponse(
            userId,
            userInfo.Value.Email,
            userInfo.Value.UserName,
            customer.FirstName,
            customer.LastName,
            customer.PhoneNumber,
            customer.DateOfBirth,
            customer.Gender);

        return Result<ProfileResponse>.Success(response);
    }
}

