using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using OrderWeb.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

IHost host = Host.CreateDefaultBuilder(args)
    .UseContentRoot(AppContext.BaseDirectory) // ensures configs are found
    .UseWindowsService()                      // run as Windows Service
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<OrderDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
