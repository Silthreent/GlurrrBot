using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                            checkSubLine(subLine);
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
            }
        }

        static void checkSubLine(string[] subLine)
        {
            switch(subLine[0])
            {
                case "name":
                    Program.discord.EditCurrentUserAsync(username: subLine[1]);
                    Program.addCallName(subLine[1]);
                    break;

                case "picture":
                    Console.WriteLine("Changing picture: " + subLine[1]);
                    try
                    {
                        Program.discord.EditCurrentUserAsync(avatar: new FileStream("characters/pictures/" + subLine[1], FileMode.Open));
                    }
                    catch(FileNotFoundException e)
                    {
                        Console.WriteLine("Picture didn't exist");
                        Console.WriteLine(e.Message);
                    }
                    break;

                case "game":
                    Program.discord.UpdateStatusAsync(game: new Game(" " + subLine[1]));
                    break;

                case "altname":
                    Program.addCallName(subLine[1]);
                    break;

                default:
                    Console.WriteLine("Invalid tag: " + subLine[1]);
                    break;
            }
        }

        static void changePicture(string file)
        {
            Console.WriteLine("Changing picture: " + file);
            try
            {
                Program.discord.EditCurrentUserAsync(avatar: new FileStream("characters/pictures/" + file, FileMode.Open));
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("Picture didn't exist");
                Console.WriteLine(e.Message);
            }
        }
    }
}
