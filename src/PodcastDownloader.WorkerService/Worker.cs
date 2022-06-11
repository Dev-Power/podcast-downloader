namespace PodcastDownloader.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly AppSettings _appSettings;
    private readonly IDownloadService _downloadService;

    public Worker(ILogger<Worker> logger, AppSettings appSettings, IDownloadService downloadService)
    {
        _logger = logger;
        _appSettings = appSettings;
        _downloadService = downloadService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(Util.FormatLogMessage($"Worker started with the following settings:\n{_appSettings}"));
        
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var podcastInfo in _appSettings.PodcastInfoList)
            {
                _downloadService.Run(podcastInfo);
            }
            
            await Task.Delay(_appSettings.CheckIntervalInDays * 60 * 60 * 24 * 1000, stoppingToken);
        }
    }
    

}