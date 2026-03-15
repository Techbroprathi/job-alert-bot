using Telegram.Bot;
using Telegram.Bot.Types;

namespace JobAlertBot.Services;

public class TelegramService
{
    private readonly TelegramBotClient bot;

    private const string BotToken = "Your-BOT-Token";
    private const long ChatId = 'Your ID';

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
