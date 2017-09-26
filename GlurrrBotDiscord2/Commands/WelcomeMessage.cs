using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2.Commands
{
    public class WelcomeMessage
    {
        public static async Task runCommand(PresenceUpdateEventArgs args)
        {
            if(File.Exists(@"welcomemessages/" + args.Member.Id + ".txt"))
            {
                Console.WriteLine("Welcome message found");
                using(StreamReader file = new StreamReader(@"welcomemessages/" + args.Member.Id + ".txt"))
                {
                    Console.WriteLine(args.Member.Username + " has a welcome message");
                    await args.Guild.Channels[0].SendMessageAsync(file.ReadLineAsync().Result);
                }
            }
        }
    }
}
