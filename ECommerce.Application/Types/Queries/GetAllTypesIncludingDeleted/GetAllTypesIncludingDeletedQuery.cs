using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Types.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Types.Queries.GetAllTypesIncludingDeleted;

public sealed record GetAllTypesIncludingDeletedQuery(
    PaginationRequest PaginationRequest)
    : IRequest<
        Result<PaginatedResult<GetAllTypesResponse>>>;