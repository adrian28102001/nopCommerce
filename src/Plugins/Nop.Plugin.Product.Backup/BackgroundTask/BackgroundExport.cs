using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nop.Plugin.Product.Backup.Services.Export;

namespace Nop.Plugin.Product.Backup.BackgroundTask;

public class BackgroundExport : IHostedService, IDisposable
{
    private int _executionCount = 0;
    private Timer _timer;

    private readonly ILogger<BackgroundExport> _logger;
    private readonly IExportService _exportService;

    public BackgroundExport(ILogger<BackgroundExport> logger,
        IExportService exportService)
    {
        _logger = logger;
        _exportService = exportService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(o =>
            {
                _exportService.Export();
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(1));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}