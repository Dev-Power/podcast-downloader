using System.Text;

namespace PodcastDownloader.WorkerService;

public class AppSettings
{
    public int CheckIntervalInDays { get; set; } = 5;
    public List<PodcastInfo> PodcastInfoList { get; set; } = new List<PodcastInfo>();

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"CheckIntervalInMinutes: {CheckIntervalInDays}");
        sb.AppendLine("Podcast to download:");
        foreach (var podcastInfo in PodcastInfoList)
        {
            sb.AppendLine($"{nameof(podcastInfo.PodcastName)}: {podcastInfo.PodcastName}, {nameof(podcastInfo.RssUrl)}: {podcastInfo.RssUrl}, {nameof(podcastInfo.LocalDirectory)}: {podcastInfo.LocalDirectory}");
        }
        return sb.ToString();
    }
}

public class PodcastInfo
{
    public string PodcastName { get; set; }
    public string RssUrl { get; set; }
    public string LocalDirectory { get; set; }
}