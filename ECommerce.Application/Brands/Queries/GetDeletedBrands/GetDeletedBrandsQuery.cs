using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Brands.Queries.GetDeletedBrands;

public sealed record GetDeletedBrandsQuery(
    PaginationRequest PaginationRequest
) : IRequest<Result<PaginatedResult<GetAllBrandsResponse>>>;