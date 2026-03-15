using HtmlAgilityPack;
using JobAlertBot.Models;
using System.Net.Http.Headers;

namespace JobAlertBot.Services;

public class LinkedInFetcher
{
    public async Task<List<JobPosting>> FetchJobs()
    {
        var jobs = new List<JobPosting>();

        var url = "https://www.linkedin.com/jobs/search/?keywords=software%20engineer&location=Bangalore";

        var http = new HttpClient();

        // Add browser headers to avoid 999 error
        http.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue("Mozilla", "5.0"));

        http.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("text/html"));

        var html = await http.GetStringAsync(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var jobCards = doc.DocumentNode.SelectNodes("//div[contains(@class,'base-card')]");

        if (jobCards == null)
            return jobs;

        foreach (var job in jobCards)
        {
            var titleNode = job.SelectSingleNode(".//h3");
            var companyNode = job.SelectSingleNode(".//h4");
            var linkNode = job.SelectSingleNode(".//a");
            var locationNode = job.SelectSingleNode(".//span[contains(@class,'job-search-card__location')]");
            var timeNode = job.SelectSingleNode(".//time");

            if (titleNode == null || companyNode == null || linkNode == null || locationNode == null)
                continue;

            var title = titleNode.InnerText.Trim();
            var company = companyNode.InnerText.Trim();
            var location = locationNode.InnerText.Trim();
            var link = linkNode.GetAttributeValue("href", "");
            var description = await FetchDescription(link);
            var postedTime = timeNode?.InnerText.Trim() ?? "";
            var jobId = ExtractJobId(link);

            jobs.Add(new JobPosting
            {
                Id = jobId,
                Title = title,
                Company = company,
                Location = location,
                Link = link,
                Description = description,
                PostedTime = postedTime
            });
        }

        return jobs;
    }
    private string ExtractJobId(string url)
    {
        var lastDash = url.LastIndexOf("-");
        if (lastDash == -1)
            return url;

        var idPart = url.Substring(lastDash + 1);

        var questionIndex = idPart.IndexOf("?");
        if (questionIndex != -1)
            idPart = idPart.Substring(0, questionIndex);

        return idPart;
    }
    private async Task<string> FetchDescription(string url)
    {
        var http = new HttpClient();

        http.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0");

        var html = await http.GetStringAsync(url);

        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(html);

        var descNode = doc.DocumentNode
            .SelectSingleNode("//div[contains(@class,'show-more-less-html')]");

        if (descNode == null)
            return "";

        return descNode.InnerText;
    }
}