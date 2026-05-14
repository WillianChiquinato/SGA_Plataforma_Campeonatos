namespace SGA_Plataforma.Worker;

using StackExchange.Redis;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnectionMultiplexer? _redis;

    public Worker(ILogger<Worker> logger, IConnectionMultiplexer? redis = null)
    {
        _logger = logger;
        _redis = redis;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_redis is not null)
        {
            try
            {
                await _redis.GetDatabase().PingAsync();
                _logger.LogInformation("Redis worker connection established.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis was configured for the worker, but the initial ping failed.");
            }
        }
        else
        {
            _logger.LogWarning("Worker started without Redis configuration.");
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
