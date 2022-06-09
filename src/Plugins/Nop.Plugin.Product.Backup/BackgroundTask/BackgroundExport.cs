﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nop.Plugin.Product.Backup.Factory;
using Nop.Services.Catalog;

namespace Nop.Plugin.Product.Backup.BackgroundTask;

public class BackgroundExport : IHostedService ,IDisposable
{
    private int _executionCount = 0;
    private readonly ILogger<BackgroundExport> _logger;
    private Timer _timer;
    private readonly IProductBackupFactory _productBackupFactory;

    public BackgroundExport(ILogger<BackgroundExport> logger, IProductBackupFactory productBackupFactory)
    {
        _logger = logger;
        _productBackupFactory = productBackupFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(o => {
                _productBackupFactory.PrepareProductBackupModel();
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(10));
 
        return Task.CompletedTask;    
    }
    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref _executionCount);

        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}