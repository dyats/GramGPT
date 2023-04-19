using GramGPT;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using GPTMessage = GramGPT.Models.Message;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var botClient = new TelegramBotClient("<bot-token>");

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
        await Console.Out.WriteLineAsync($"\nUser: {update.Message.Chat.FirstName}, message: {message.Text}");

        string apiKey = "<api-key>";
        var messages = new List<GPTMessage>
        {
            new GPTMessage { Role = "system", Content = "You are a helpful assistant." },
            new GPTMessage { Role = "user", Content = message.Text }
        };

        string responseText = await GPTService.CallChatGPTAsync(apiKey, messages);
        await Console.Out.WriteLineAsync($"Response from chat: {responseText}");

        await botClient.SendTextMessageAsync(message.Chat.Id, $"Response from chat:\n\n{responseText}");
    }
}

async Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{

    await Console.Out.WriteLineAsync($"Error occurred: {exception.Message}");
}
