using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderWeb.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<Worker> _logger;

    public Worker(IServiceProvider services, ILogger<Worker> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("OrderProcessor Worker started at: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                    var pendingOrders = await db.Orders
                        .Where(o => o.Status == "Pending")
                        .ToListAsync(stoppingToken);

                    foreach (var order in pendingOrders)
                    {
                        order.Status = "Processed";
                        _logger.LogInformation($"Order {order.Id} processed at {DateTime.Now}");
                    }

                    await db.SaveChangesAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing orders");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }

        _logger.LogInformation("OrderProcessor Worker stopping at: {time}", DateTimeOffset.Now);
    }
}
