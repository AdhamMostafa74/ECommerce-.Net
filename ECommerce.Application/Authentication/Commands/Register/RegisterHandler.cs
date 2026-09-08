
using ECommerce.Application.Authentication.Errors;
using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Authentication.Commands.Register;

public sealed class RegisterHandler(
    IIdentityService identityService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterCommand, Result<Guid>>
{
    private readonly IIdentityService _identityService = identityService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _identityService.CreateUserAsync(
            request.Email,
            request.UserName,
            request.Password,
            cancellationToken);

        if (!result.Success)
        {
            return Result<Guid>.Failure(
                result.Error!);
        }

        var customer = Customer.Create(
            result.UserId,
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.DateOfBirth,
            request.Gender);

        var customerRepository =
            _unitOfWork.Repository<Customer>();

        customerRepository.Create(customer);

        try
        {
            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
        }
        catch
        {
            await _identityService.DeleteUserAsync(
                result.UserId,
                cancellationToken);

            return Result<Guid>.Failure(
                AuthenticationErrors.RegistrationFailed);
        }

        return Result<Guid>.Success(result.UserId);
    }
}

