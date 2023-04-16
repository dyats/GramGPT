using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var botClient = new TelegramBotClient("<bot:token>");

using var cts = new CancellationTokenSource();

botClient.StartReceiving(
    updateHandler: HandleUpdate,
    pollingErrorHandler: HandleError,
    cancellationToken: cts.Token
);

Console.WriteLine("Bot started... Press Enter to stop");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type == UpdateType.Message && update.Message.Type == MessageType.Text)
    {
        var message = update.Message;
        await botClient.SendTextMessageAsync(message.Chat.Id, $"You said: {message.Text}");
    }
}

async Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    Console.WriteLine($"Error occurred: {exception.Message}");
}
