using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Account.Queries.GetProfile;

public sealed record GetProfileQuery
    : IRequest<Result<ProfileResponse>>;

