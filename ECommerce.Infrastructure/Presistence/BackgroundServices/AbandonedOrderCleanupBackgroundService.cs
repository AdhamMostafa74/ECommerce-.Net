using ECommerce.Domain.Entities.Orders;
using ECommerce.Infrastructure.Presistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Presistence.BackgroundServices;

public sealed class AbandonedOrderCleanupBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<AbandonedOrderCleanupBackgroundService> logger)
    : BackgroundService
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromHours(1);
    private static readonly TimeSpan OrderTimeout = TimeSpan.FromHours(24);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("AbandonedOrderCleanupBackgroundService started.");

        using var timer = new PeriodicTimer(CheckInterval);

        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await CleanupAbandonedOrdersAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error occurred during abandoned orders cleanup.");
            }
        }

        logger.LogInformation("AbandonedOrderCleanupBackgroundService stopped.");
    }

    private async Task CleanupAbandonedOrdersAsync(CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();

        var cutoff = DateTimeOffset.UtcNow - OrderTimeout;

        var abandonedOrders = await dbContext.Orders
            .Include(o => o.Payments)
            .Where(o =>
                o.PaymentMethod == PaymentMethod.Online &&
                o.PaymentStatus == PaymentStatus.Pending &&
                o.Status == OrderStatus.Pending &&
                o.CreatedAt < cutoff)
            .Take(100)
            .ToListAsync(ct);

        if (abandonedOrders.Count == 0)
            return;

        foreach (var order in abandonedOrders)
        {
            order.Cancel();
            order.MarkPaymentAsFailed();

            foreach (var payment in order.Payments.Where(p => p.Status == PaymentStatus.Pending))
            {
                payment.MarkAsFailed();
            }
        }

        await dbContext.SaveChangesAsync(ct);

        logger.LogInformation(
            "Cancelled {Count} abandoned online order(s) created before {Cutoff}.",
            abandonedOrders.Count,
            cutoff);
    }
}

