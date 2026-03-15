using JobAlertBot.Services;
using JobAlertBot.Storage;

namespace JobAlertBot
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var fetcher = new LinkedInFetcher();
            var telegram = new TelegramService();
            var filter = new JobFilterService();
            var store = new SeenJobStore();


            var jobs = await fetcher.FetchJobs();

            foreach (var job in jobs)
            {
                if (!filter.IsAllowed(job))
                    continue;

                if (!store.IsNew(job.Id))
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

                await telegram.SendMessage(message);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(10));
            }
        }
    }
}
