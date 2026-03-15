using JobAlertBot;
using JobAlertBot.Services;
using JobAlertBot.Storage;

var builder = Host.CreateApplicationBuilder(args);

// Register Worker
builder.Services.AddHostedService<Worker>();

// Register Services
builder.Services.AddSingleton<LinkedInFetcher>();
builder.Services.AddSingleton<TelegramService>();
builder.Services.AddSingleton<JobFilterService>();
builder.Services.AddSingleton<SeenJobStore>();

var host = builder.Build();
host.Run();