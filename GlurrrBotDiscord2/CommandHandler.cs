using DSharpPlus.EventArgs;
using GlurrrBotDiscord2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2
{
    public class CommandHandler
    {
        public static bool japanMode = false;

        public static async Task handleCommand(MessageCreateEventArgs args)
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
        }
    }
}
