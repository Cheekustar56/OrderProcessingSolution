using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderWeb.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;  // for Where(), OrderBy(), etc.



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
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                var pendingOrders = await db.Orders
                    .Where(o => o.Status == "Pending")
                    .ToListAsync();

                foreach (var order in pendingOrders)
                {
                    order.Status = "Processed";
                    _logger.LogInformation($"Order {order.Id} processed at {DateTime.Now}");
                }

                await db.SaveChangesAsync();
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
