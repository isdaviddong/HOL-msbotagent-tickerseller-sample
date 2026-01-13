// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.22.0

using isRock.Template;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot1.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //var replyText = isRock.Template.ChatGPT.getResponseFromGPT(turnContext.Activity.Text,.....);
            //var replyText = $"Echo: {turnContext.Activity.Text}";

            var replyText = "";

            if (turnContext.Activity.Text.Contains("/reset"))
            {
                ChatHistoryManager.DeleteIsolatedStorageFile();
                replyText = "我已經把之前的對談都給忘了!";
            }
            else
            {
                var chatHistory = ChatHistoryManager.GetMessagesFromIsolatedStorage("UserA");
                //var replyText = $"Echo: {turnContext.Activity.Text}";
                replyText = ChatGPT.getResponseFromGPT(turnContext.Activity.Text, chatHistory);
                //儲存聊天紀錄
                ChatHistoryManager.SaveMessageToIsolatedStorage(
                    DateTime.Now, "UserA", turnContext.Activity.Text, replyText);
            }

            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }


        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
