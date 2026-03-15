using JobAlertBot;
using JobAlertBot.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<TelegramService>();

var host = builder.Build();
host.Run();
