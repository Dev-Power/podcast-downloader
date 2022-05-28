namespace PodcastDownloader.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly AppSettings _appSettings;
    
    public Worker(ILogger<Worker> logger, AppSettings appSettings)
    {
        _logger = logger;
        _appSettings = appSettings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(FormatLogMessage($"Worker started with the following settings:\n{_appSettings}"));
        
        var downloadService = new DownloadService();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var podcastInfo in _appSettings.PodcastInfoList)
            {
                downloadService.Run(podcastInfo);
            }
            
            await Task.Delay(_appSettings.CheckIntervalInDays * 60 * 60 * 24 * 1000, stoppingToken);
        }
    }
    
    private string FormatLogMessage(string message)
    {
        return $"[{DateTime.UtcNow.ToString()}] {message}";
    }
}