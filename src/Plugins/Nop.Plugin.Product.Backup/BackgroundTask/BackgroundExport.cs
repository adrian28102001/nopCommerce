using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nop.Services.Catalog;

namespace Nop.Plugin.Product.Backup.BackgroundTask;

public class BackgroundExport : IHostedService ,IDisposable
{
    private readonly ILogger<BackgroundExport> _logger;
    private Timer _timer;
    private readonly IProductService _productService;

    public BackgroundExport(ILogger<BackgroundExport> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(o => {
                _logger.LogInformation($"Printing the worker number");
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(5));
 
        return Task.CompletedTask;    
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}