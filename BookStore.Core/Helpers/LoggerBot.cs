using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace BookStore.Core.Helpers;

public static class LoggerBot
{
    public static async void Log(string message, LogType logType)
    {
        var botClient = new TelegramBotClient("Your token");
        var chatId = -100;//channelId;
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