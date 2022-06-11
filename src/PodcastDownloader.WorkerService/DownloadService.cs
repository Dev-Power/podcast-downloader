using System.Xml;
using System.Net;

namespace PodcastDownloader.WorkerService;

public interface IDownloadService
{
    void Run(PodcastInfo podcastInfo);
}

public class DownloadService : IDownloadService
{
    private readonly ILogger<DownloadService> _logger;
    
    public DownloadService(ILogger<DownloadService> logger)
    {
        _logger = logger;
    }
    
    public void Run(PodcastInfo podcastInfo)
    {
        _logger.LogInformation(Util.FormatLogMessage($"Running for podcast {podcastInfo.PodcastName}"));
        
        if (podcastInfo.RssUrl.Length == 0) { throw new InvalidOperationException("Nothing to download"); }
        
        string feedUrl = podcastInfo.RssUrl;
        string targetLocalDirectory = podcastInfo.LocalDirectory.TrimEnd('/');
        string feedLocalFileNnme = "temp-feed.xml";
        
        using (var client = new WebClient())
        {
            _logger.LogInformation(Util.FormatLogMessage($"Downloading {feedUrl} to {feedLocalFileNnme}"));
            client.DownloadFile(feedUrl, feedLocalFileNnme);
        }

        string rawXml = File.ReadAllText(feedLocalFileNnme);
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(rawXml);

        XmlNodeList itemNodeList = xmlDocument.SelectNodes("/rss/channel/item");

        for (int i = 0; i < itemNodeList.Count; i++)
        {
            XmlNode titleNode = itemNodeList[i].SelectNodes("title")[0];
            XmlNode enclosureNode = itemNodeList[i].SelectNodes("enclosure")[0];
            XmlNode pubDateNode = itemNodeList[i].SelectNodes("pubDate")[0];
            string urlToDownload = enclosureNode.Attributes["url"].Value;
            DateTime pubDate = DateTime.Parse(pubDateNode.InnerText);
            string localFileName = $"{targetLocalDirectory}/{GetDate(pubDate)}-{GetTitle(titleNode.InnerText)}.mp3";
            if (!File.Exists(localFileName))
            {
                using (var client = new WebClient())
                {
                    _logger.LogInformation(Util.FormatLogMessage($"Downloading {urlToDownload} to {localFileName}"));
                    client.DownloadFile(urlToDownload, localFileName);
                    Thread.Sleep(2000);
                }
            }
            else
            {
                // Console.WriteLine($"Skipping. File at {localFileName} already exists.");
            }
    
            string GetDate(DateTime pubDate)
            {
                return $"{pubDate.Year}-{pubDate.Month.ToString().PadLeft(2, '0')}-{pubDate.Day.ToString().PadLeft(2, '0')}";
            }

            string GetTitle(string rawTitle)
            {
                return rawTitle.Replace(":", "-");
            }
        }

        File.Delete(feedLocalFileNnme);
    }
}