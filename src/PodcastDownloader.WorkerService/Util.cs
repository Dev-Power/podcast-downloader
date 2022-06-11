namespace PodcastDownloader.WorkerService;

public class Util
{
    public static string FormatLogMessage(string message)
    {
        return $"[{DateTime.UtcNow.ToString()}] {message}";
    }
}