using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.VoiceNext;
using GlurrrBotDiscord2.Commands;
using System;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2
{
    public class CommandHandler
    {
        public static async Task messageCreated(MessageCreateEventArgs args)
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

            if(msg.Contains("kys"))
            {
                Console.WriteLine("Running Keep Yourself Safe (MessageCreated)");
                await KeepYourselfSafe.runCommand(args);
            }

            if(msg.Contains("anime"))
            {
                Console.WriteLine("Running Anime (MessageCreated)");
                await args.Message.RespondAsync(Character.getText("anime"));
            }

            /*if(msg.Contains("connect"))
            {
                Task voiceConnect = new Task(async () =>
                {
                    Console.WriteLine("Connecting");
                    await Program.discord.GetVoiceNextClient().ConnectAsync(args.Guild.GetChannel(332256997644959745));
                    Console.WriteLine("Connected");
                });
                voiceConnect.Start();
            }*/
        }

        public static async Task presenceUpdated(PresenceUpdateEventArgs args)
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
