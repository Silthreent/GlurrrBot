using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using DSharpPlus.Net.WebSocket;
using DSharpPlus.VoiceNext;
using DSharpPlus.VoiceNext.Codec;
using GlurrrBotDiscord2.Commands;
using System;
using System.IO;
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

        const string VERSION_NUMBER = "0.9.1-12";

        public static DiscordClient discord;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Running GlurrrBot V" + VERSION_NUMBER);
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

            PoemGameDictionary.buildDictionary();

            discord.MessageCreated += onMessageCreated;

            discord.PresenceUpdated += CommandHandler.presenceUpdated;
            //discord.VoiceStateUpdated += voiceStateUpdated;

            discord.MessageReactionAdded += CommandHandler.reactionAdded;

            discord.GuildAvailable += init;

            //discord.SetWebSocketClient<WebSocketSharpClient>();
            VoiceNextConfiguration vcfg = new VoiceNextConfiguration()
            {
                EnableIncoming = false
            };
            //discord.UseVoiceNext(vcfg);

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
                        subLine = line.Split('|');
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
                                await Program.discord.UpdateStatusAsync(game: new Game(subLine[1]));
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
            Console.WriteLine(e.Message.Content.ToLower());

            if(await checkFixedCommands(e))
                return;

            foreach(string name in Character.callNames)
            {
                if(e.Message.Content.ToLower().Contains(" " + name) || e.Message.Content.ToLower().Contains(" " + name + " ") || e.Message.Content.ToLower().Contains(name + " "))
                {
                    Console.WriteLine("Glurrr awakened");
                    await CommandHandler.messageCreated(e);
                }
            }

            if(e.Message.Content.Contains("ぐるる"))
            {
                Console.WriteLine("グルーラーが目を覚ました");

                await CommandHandler.messageCreated(e);
                return;
            }
        }

        private static async Task voiceStateUpdated(VoiceStateUpdateEventArgs e)
        {
            if(e.User.IsBot == true)
            {
                Console.WriteLine("Bot moved");
                return;
            }
            if(e.Channel == null)
            {
                Console.WriteLine(e.User.Username + " left a voice channel");
                try
                {
                    discord.GetVoiceNextClient().GetConnection(e.Guild).Disconnect();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return;
            }

            Console.WriteLine(e.User.Username + " joined voice channel " + e.Channel.Name);

            Task voiceStateUpdated = new Task(async () => await CommandHandler.voiceStateUpdated(e));
            voiceStateUpdated.Start();
        }

        private static async Task<bool> checkFixedCommands(MessageCreateEventArgs e)
        {
            if(e.Message.Content.StartsWith("renpy.file(\"characters/") && e.Message.Content.EndsWith(")"))
            {
                //await ChangeCharacter.runCommand(e);
                Task changeChr = new Task(async () => await ChangeCharacter.runCommand(e));
                changeChr.Start();
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

                await e.Channel.SendMessageAsync(embed: embed);
                return true;
            }

            return false;
        }

        public static async Task update(string name, string avatar, string game, DiscordGuild guild, string roleName)
        {
            try
            {
                await discord.EditCurrentUserAsync(username: name, avatar: new FileStream("characters/pictures/" + avatar, FileMode.Open));

                await discord.UpdateStatusAsync(game: new Game(game));

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
