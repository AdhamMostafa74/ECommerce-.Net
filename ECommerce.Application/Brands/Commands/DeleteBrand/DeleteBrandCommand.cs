using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Brands.Commands.DeleteBrand;

public sealed record DeleteBrandCommand(Guid Id)
    : IRequest<Result<Unit>>;