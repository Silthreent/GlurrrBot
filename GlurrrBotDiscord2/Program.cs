using DSharpPlus;
using DSharpPlus.EventArgs;
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
        static DiscordClient discord;

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
                Console.WriteLine("wtf happens");
                Console.WriteLine(e.Message);
            }

            discord.MessageCreated += onMessageCreated;
            MessageCreated += CommandHandler.handleCommand;

            await discord.ConnectAsync();

            await Task.Delay(-1);
        }

        private static async Task onMessageCreated(MessageCreateEventArgs e)
        {
            if(e.Message.Content.ToLower().Contains("glurrr") || e.Message.Content.ToLower().Contains("glibba"))
            {
                Console.WriteLine("Glurrr awakened");
                await MessageCreated(e);
            }
        }
    }
}
