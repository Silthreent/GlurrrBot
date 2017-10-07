using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using GlurrrBotDiscord2.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2
{
    public class CommandHandler
    {
        public static async Task messageCreatedCommand(MessageCreateEventArgs args)
        {
            string msg = args.Message.Content.ToLower();

            if(msg.Contains("randome"))
            {
                Console.WriteLine("Running Randome (MessageCreated)");
                await Randome.runCommand(args);
            }

            if(msg.Contains("welcome"))
            {
                Console.WriteLine("Running Welcome (MessageCreated)");
                await WelcomeMessage.updateMessages(args);
            }

            if(msg.Contains("anime"))
            {
                Console.WriteLine("Running Anime (MessageCreated)");
                await args.Message.RespondAsync(Character.getText("anime"));
            }
        }

        public static async Task presenceUpdatedCommand(PresenceUpdateEventArgs args)
        {
            Console.WriteLine(args.Member.Username + " : Before - " + args.PresenceBefore.Status + " : After - " + args.Member.Presence.Status);

            if(args.PresenceBefore.Status == UserStatus.Offline && args.Member.Presence.Status == UserStatus.Online)
            {
                Console.WriteLine("User just came online, running command");
                await WelcomeMessage.welcomeMessage(args);
            }
        }
    }
}
