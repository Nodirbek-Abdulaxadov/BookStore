using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace BookStore.Core.Helpers;

public static class LoggerBot
{
    public static async void Log(string message, LogType logType)
    {
        var botClient = new TelegramBotClient("6595345855:AAHiQfCrL_yVy0yrovqTrpS71TI9iR45MqA");
        var chatId = -1001917771086;
        var smile = logType switch
        {
            LogType.Info => "✅",
            LogType.Warning => "⚠️",
            LogType.Error => "❌",
            _ => "❓"
        };

         await botClient.SendTextMessageAsync(
         chatId: chatId,
         text: $"""
         [{smile} {logType}] : {TimeUz.Now}
         
         {message}
         """,
         parseMode: ParseMode.Markdown,
         disableNotification: true);
    }
}

public enum LogType
{
    Info,
    Warning,
    Error
}