using ECommerce.Application.Common.Files;
using ECommerce.Domain.Common.Results;
using MediatR;

public sealed record UpdateProductPictureCommand(
    Guid ProductId,
    FileUpload Picture
) : IRequest<Result>;