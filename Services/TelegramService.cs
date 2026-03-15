using Telegram.Bot;
using Telegram.Bot.Types;

namespace JobAlertBot.Services;

public class TelegramService
{
    private readonly TelegramBotClient bot;
    private readonly long chatId;

    public TelegramService(IConfiguration config)
    {
        var token = config["Telegram:BotToken"];
        chatId = long.Parse(config["Telegram:ChatId"]);

        bot = new TelegramBotClient(token);
    }
    public async Task SendMessage(string message)
    {
        await bot.SendMessage(
            chatId: ChatId,
            text: message
        );
    }
}
