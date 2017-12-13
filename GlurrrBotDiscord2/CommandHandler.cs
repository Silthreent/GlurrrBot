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
        static VoiceNextConnection voiceConnection;

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

            if(msg.Contains("write") && msg.Contains("poem"))
            {
                Console.WriteLine("Running Poem Game (MessageCreated)");
                await PoemGame.runCommand(args);
            }

            if(msg.Contains("get"))
            {
                Console.WriteLine("Running Message Getter (MessageCreated)");
                await MessageSaver.getMessage(args);
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

        public static async Task reactionAdded(MessageReactionAddEventArgs args)
        {
            if(args.User.IsBot)
                return;

            if(args.Emoji.Name == "monika" || args.Emoji.Name == "sayori" || args.Emoji.Name == "natsuki" || args.Emoji.Name == "yuri")
            {
                await PoemGame.reactionAdded(args);
            }

            if(args.Emoji.Name == "glurrr")
            {
                Console.WriteLine("Saving message");
                await MessageSaver.reactionAdded(args);
            }
        }

        public static async Task voiceStateUpdated(VoiceStateUpdateEventArgs e)
        {
            try
            {
                await Program.discord.GetVoiceNextClient().ConnectAsync(e.Channel);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Voice connected");
        }
    }
}
