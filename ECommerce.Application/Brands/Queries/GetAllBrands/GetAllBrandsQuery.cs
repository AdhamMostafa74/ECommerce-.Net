using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Brands.Queries.GetAllBrands;

public sealed record GetAllBrandsQuery(
    PaginationRequest PaginationRequest
) : IRequest<Result<PaginatedResult<GetAllBrandsResponse>>>;