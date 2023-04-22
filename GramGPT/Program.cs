using GramGPT;
using GramGPT.Models;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using GPTMessage = GramGPT.Models.Message;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

string apiKey = "<api-key>";

var botClient = new TelegramBotClient("<bot-token>");

using var cts = new CancellationTokenSource();

var conversationHistory = new List<GPTMessage>
{
    new GPTMessage { Role = Role.System, Content = "You are a helpful assistant." },
};

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
        // Get user's message and add it to conversation history
        var message = update.Message;
        conversationHistory.Add(new GPTMessage { Role = Role.User, Content = message.Text });
        await Console.Out.WriteLineAsync($"\nUser: {update.Message.Chat.FirstName}, message: {message.Text}");

        // indicates that request is IN PROGRESS
        await botClient.SendChatActionAsync(update.Message.Chat.Id, ChatAction.Typing, cancellationToken);

        // Get response from GPT and add it to conversation history
        string responseText = await GPTService.CallChatGPTAsync(apiKey, conversationHistory);
        conversationHistory.Add(new GPTMessage { Role = Role.Assistant, Content = responseText });
        await Console.Out.WriteLineAsync($"Response from chat: {responseText}");

        await botClient.SendTextMessageAsync(message.Chat.Id, $"Response from chat:\n\n{responseText}");
    }
}

async Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{

    await Console.Out.WriteLineAsync($"Error occurred: {exception.Message}");
}
