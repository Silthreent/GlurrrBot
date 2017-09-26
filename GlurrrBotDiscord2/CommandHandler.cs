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
        public static bool japanMode = false;

        public static async Task messageCreatedCommand(MessageCreateEventArgs args)
        {
            string msg = args.Message.Content.ToLower();

            if(msg.Contains("randome") || msg.Contains("らんどめ"))
            {
                await Randome.runCommand(args);
            }

            if(msg.Contains("anime"))
            {
                Console.WriteLine("Running anime");
                await args.Message.RespondAsync("I love 2d");
            }

            if(msg.Contains("picture"))
            {
                Console.WriteLine("Running pictures");
                await AnimePicture.runCommand(args);
            }
        }

        public static async Task presenceUpdatedCommand(PresenceUpdateEventArgs args)
        {
            Console.WriteLine(args.Member.Username + " : Before - " + args.PresenceBefore.Status + " : After - " + args.Member.Presence.Status);

            if(args.PresenceBefore.Status != UserStatus.Online && args.Member.Presence.Status == UserStatus.Online)
            {
                Console.WriteLine("User just came online, running command");
                await WelcomeMessage.runCommand(args);
            }
        }
    }
}
