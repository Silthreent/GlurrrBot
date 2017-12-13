using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2.Commands
{
    class MessageSaver
    {
        public static async Task reactionAdded(MessageReactionAddEventArgs args)
        {
            SQLManager.saveMessage(args.Message.Id, "Test");
            await args.Channel.SendMessageAsync("Saved message " + args.Message.Id);
        }

        public static async Task getMessage(MessageCreateEventArgs args)
        {
            string[] split = args.Message.Content.Split('"');
            if(split.Length < 3)
            {
                await args.Channel.SendMessageAsync(Character.getText("noquotes"));
                Console.WriteLine("No quotes entered");
                return;
            }

            List<ulong> reader = SQLManager.getMessagesByTag(split[1]);
            if(reader.Count < 1)
            {
                await args.Channel.SendMessageAsync(Character.getText("emptytag"));
                Console.WriteLine("Nothing in specified tag");
                return;
            }

            string builder = "";
            try
            {
                DiscordMessage message;
                foreach(ulong messageID in reader)
                {
                    message = args.Channel.GetMessageAsync(messageID).Result;
                    Console.WriteLine(message.Attachments.Count);
                    if(message.Attachments.Count >= 1)
                    {
                        builder += "| " + message.Attachments[0].Url + " |";
                    }
                    else
                    {
                        builder += "| " + message.Content + " |";
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException.Message);
            }

            await args.Channel.SendMessageAsync(builder);
        }
    }
}
