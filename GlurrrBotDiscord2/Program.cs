using DSharpPlus;
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
    class Program
    {
        //public const ulong MATT_ID = 134852512611172352;
        //public const ulong DAVID_ID = 135498846494130177;
        // os.remove("characters/yuri.chr")
        // renpy.file("characters/monika.chr")
        // monika.chr does not exist.
        // yuri.chr deleted successfully.

        public static DiscordClient discord;
        static List<string> callNames = new List<string>();

        static event AsyncEventHandler<MessageCreateEventArgs> MessageCreated;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            try
            {
                using(StreamReader sr = new StreamReader("botcode.txt"))
                {
                    discord = new DiscordClient(new DiscordConfiguration
                    {
                        Token = sr.ReadLine(),
                        TokenType = TokenType.Bot,
                        UseInternalLogHandler = true,
                        LogLevel = LogLevel.Debug
                    });
                }
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("Code file not found");
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine("wtf happened");
                Console.WriteLine(e.Message);
            }

            discord.MessageCreated += onMessageCreated;
            MessageCreated += CommandHandler.messageCreatedCommand;

            discord.PresenceUpdated += CommandHandler.presenceUpdatedCommand;

            await discord.ConnectAsync();

            await Task.Delay(-1);
        }

        private static async Task onMessageCreated(MessageCreateEventArgs e)
        {
            await checkFixedCommands(e);

            if(e.Message.Content.ToLower().Contains("glurrr") || e.Message.Content.ToLower().Contains("glibba"))
            {
                Console.WriteLine("Glurrr awakened");
                CommandHandler.japanMode = false;
                await MessageCreated(e);
            }

            if(e.Message.Content.Contains("ぐるる"))
            {
                Console.WriteLine("グルーラーが目を覚ました");
                CommandHandler.japanMode = true;
                await MessageCreated(e);
            }
        }

        private static async Task checkFixedCommands(MessageCreateEventArgs e)
        {
            if(e.Message.Content.StartsWith("renpy.file(\"characters/") && e.Message.Content.EndsWith(")"))
            {
                await ChangeCharacter.runCommand(e);
                return;
            }

            if(e.Message.Content.ToLower() == "/leave")
            {
                var embed = new DiscordEmbedBuilder()
                {
                    Title = e.Author.Username + " has left",
                    Description = e.Author.Username + " has left the Discord and would like everyone to know they did. They are very triggered.",
                    Color = DiscordColor.DarkRed,
                };

                await e.Channel.SendMessageAsync("", false, embed);
                return;
            }
        }

        public static void addCallName(string name)
        {
            if(!(callNames.Contains(name)))
                callNames.Add(name);
        }

        public static void clearCallNames()
        {
            callNames.Clear();
        }
    }
}
