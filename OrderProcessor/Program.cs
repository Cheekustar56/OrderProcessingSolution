using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using OrderWeb.Data;
using Microsoft.Extensions.Configuration;
using System;

var host = Host.CreateDefaultBuilder(args)
    // Ensure the host looks for configuration files in the folder where the DLL is deployed
    .UseContentRoot(AppContext.BaseDirectory)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.SetBasePath(AppContext.BaseDirectory);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<OrderDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
