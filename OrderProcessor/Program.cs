using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using OrderWeb.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .UseWindowsService() // Enables running as a Windows service
    .ConfigureServices((context, services) =>
    {
        // Configure EF Core DbContext
        services.AddDbContext<OrderDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

        // Register the Worker
        services.AddHostedService<Worker>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole(); // You can add file logging if needed
    })
    .Build();

await host.RunAsync();
