using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2
{
    public class CommandHandler
    {
        public static async Task handleCommand(MessageCreateEventArgs args)
        {
            string msg = args.Message.Content.ToLower();

            if(msg.Contains("anime"))
            {
                Console.WriteLine("Running anime");
                await args.Message.RespondAsync("I love 2d");
            }
        }
    }
}
