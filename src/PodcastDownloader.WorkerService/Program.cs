using PodcastDownloader.WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddSingleton(x => configuration.GetSection("AppSettings").Get<AppSettings>());
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();