using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
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
        // TODO: Change some data to be in a different file, such as client.
        // TODO: Have any messages read from a global source, based on character set
        //public const ulong MATT_ID = 134852512611172352;
        //public const ulong DAVID_ID = 135498846494130177;
        // os.remove("characters/yuri.chr")
        // renpy.file("characters/monika.chr")
        // monika.chr does not exist.
        // yuri.chr deleted successfully.

        public static DiscordClient discord;

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

            discord.GuildAvailable += init;

            await discord.ConnectAsync();

            await Task.Delay(-1);
        }

        private static async Task init(GuildCreateEventArgs e)
        {
            await Character.loadDefault();
            Console.WriteLine("Loading call names");

            string[] subLine;
            string line;
            string welcome = " ";

            try
            {
                using(StreamReader file = new StreamReader(@"characters/" + discord.CurrentUser.Username.ToLower() + ".chr"))
                {
                    while((line = await file.ReadLineAsync()) != null)
                    {
                        subLine = line.Split(':');
                        if(subLine.Length == 2)
                        {
                            if(subLine[0] == "name" || subLine[0] == "altname")
                            {
                                Character.addCallName(subLine[1]);
                            }
                            if(subLine[0] == "welcome")
                            {
                                welcome = subLine[1];
                            }
                            if(subLine[0] == "game")
                            {
                                await Program.discord.UpdateStatusAsync(game: new Game(" " + subLine[1]));
                            }
                        }
                        else if(line == "#Text")
                        {
                            await Character.updateText(file);
                        }
                        else
                            Console.WriteLine("Invalid line " + line);
                    }
                }
            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine("Can't find character file");
                Console.WriteLine(ex.Message);
                Character.addCallName("glurrr");
                await e.Guild.GetDefaultChannel().SendMessageAsync("Can't load .chr file; added \"glurrr\" as a default call");
                return;
            }

            Console.WriteLine("Loaded call names");
            await e.Guild.GetDefaultChannel().SendMessageAsync(welcome);
        }

        private static async Task onMessageCreated(MessageCreateEventArgs e)
        {
            if(await checkFixedCommands(e))
                return;

            foreach(string name in Character.callNames)
            {
                if(e.Message.Content.ToLower().Contains(name))
                {
                    Console.WriteLine("Glurrr awakened");
                    await MessageCreated(e);
                    return;
                }
            }

            if(e.Message.Content.Contains("ぐるる"))
            {
                Console.WriteLine("グルーラーが目を覚ました");
                await MessageCreated(e);
                return;
            }
        }

        private static async Task<bool> checkFixedCommands(MessageCreateEventArgs e)
        {
            if(e.Message.Content.StartsWith("renpy.file(\"characters/") && e.Message.Content.EndsWith(")"))
            {
                await ChangeCharacter.runCommand(e);
                return true;
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
                return true;
            }

            return false;
        }

        public static async Task update(string name, string avatar, string game, DiscordGuild guild, string roleName)
        {
            try
            {
                await discord.EditCurrentUserAsync(username: name, avatar: new FileStream("characters/pictures/" + avatar, FileMode.Open));

                await discord.UpdateStatusAsync(game: new Game(" " + game));

                if(roleName != null)
                {
                    await guild.UpdateRoleAsync(guild.Roles[guild.Roles.Count - 2], name: roleName);
                }
            }
            catch(BadRequestException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
