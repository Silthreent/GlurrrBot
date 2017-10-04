using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2.Commands
{
    public class ChangeCharacter
    {
        // Find a .chr file entered, load it
        // .chr file will contain what to change the bot's name too, what it's called internally
        // it's nickname, profile pic, and anything else I could need
        public static async Task runCommand(MessageCreateEventArgs args)
        {
            Console.WriteLine("Running Change Character command");

            string chrName = args.Message.Content.Substring(23);
            chrName = chrName.Remove(chrName.Length - 2);
            Console.WriteLine("Loading: " + chrName);
            await args.Channel.SendMessageAsync("Loading " + chrName + "...");

            string line;
            string[] subLine;

            try
            {
                using(StreamReader file = new StreamReader(@"characters/" + chrName))
                {
                    Program.clearCallNames();

                    while((line = await file.ReadLineAsync()) != null)
                    {
                        subLine = line.Split(':');
                        if(subLine.Length == 2)
                            await checkSubLine(subLine, args.Guild);
                        else
                            Console.WriteLine("Invalid line " + line);
                    }
                }
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("Character file did not exist");
                Console.WriteLine(e.Message);
                await args.Channel.SendMessageAsync(chrName + " does not exist.");
                return;
            }

            await args.Channel.SendMessageAsync(chrName + " loaded successfully.");
        }

        public static void changeCharacter()
        {

        }

        static async Task checkSubLine(string[] subLine, DiscordGuild guild)
        {
            Console.WriteLine(subLine[0] + " : " + subLine[1]);
            try
            {
                switch(subLine[0])
                {
                    case "name":
                        await Program.discord.EditCurrentUserAsync(username: subLine[1]);
                        Program.addCallName(subLine[1]);
                        break;

                    case "picture":
                        Console.WriteLine("Changing picture: " + subLine[1]);
                        await Program.discord.EditCurrentUserAsync(avatar: new FileStream("characters/pictures/" + subLine[1], FileMode.Open));
                        break;

                    case "game":
                        await Program.discord.UpdateStatusAsync(game: new Game(" " + subLine[1]));
                        break;

                    case "altname":
                        Program.addCallName(subLine[1]);
                        break;

                    case "role":
                        await guild.UpdateRoleAsync(guild.Roles[guild.Roles.Count - 2], name: subLine[1]);
                        break;

                    default:
                        Console.WriteLine("Invalid tag: " + subLine[1]);
                        break;
                }
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("File didn't exist");
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
