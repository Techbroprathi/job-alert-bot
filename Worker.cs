using JobAlertBot.Services;
using JobAlertBot.Storage;

namespace JobAlertBot
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly LinkedInFetcher _fetcher;
        private readonly TelegramService _telegram;
        private readonly JobFilterService _filter;
        private readonly SeenJobStore _store;

        public Worker(
            ILogger<Worker> logger,
            LinkedInFetcher fetcher,
            TelegramService telegram,
            JobFilterService filter,
            SeenJobStore store)
        {
            _logger = logger;
            _fetcher = fetcher;
            _telegram = telegram;
            _filter = filter;
            _store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var jobs = await _fetcher.FetchJobs();

                    foreach (var job in jobs)
                    {
                        if (!_filter.IsAllowed(job))
                            continue;

                        if (!_store.IsNew(job.Id))
                            continue;

                        var message =
                        $"""
                        🚀 New Job Alert

                        Company: {job.Company}
                        Role: {job.Title}
                        Location: {job.Location}
                        Posted on: {job.PostedTime}

                        Apply Here:
                        {job.Link}

                        Source: LinkedIn
                        """;

                        await _telegram.SendMessage(message);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing jobs");
                }

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}