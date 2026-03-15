using Telegram.Bot;
using Telegram.Bot.Types;

namespace JobAlertBot.Services;

public class TelegramService
{
    private readonly TelegramBotClient bot;

    private const string BotToken = "8288945933:AAGBTS3cjF1NhF_U6LgSBbNoY9iywmxI6jc";
    private const long ChatId = 5649162235;

    public TelegramService()
    {
        bot = new TelegramBotClient(BotToken);
    }

    public async Task SendMessage(string message)
    {
        await bot.SendMessage(
            chatId: ChatId,
            text: message
        );
    }
}