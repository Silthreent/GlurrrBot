using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
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

        static DateTime cooldown;

        public static async Task runCommand(MessageCreateEventArgs args)
        {
            Console.WriteLine("Running Change Character command");

            if(cooldown.CompareTo(DateTime.UtcNow) > 0)
            {
                Console.WriteLine("Ratelimit locked");
                await args.Channel.SendMessageAsync(Character.getText("characterdelay", (cooldown - DateTime.UtcNow).Minutes.ToString()));
                return;
            }

            string chrName = args.Message.Content.Substring(23);
            chrName = chrName.Remove(chrName.Length - 2);
            Console.WriteLine("Loading: " + chrName);
            await args.Channel.SendMessageAsync(Character.getText("loadchr.", chrName));

            string line;
            string[] subLine;
            string name = "";
            string picture = "";
            string game = "";
            string roleName = "";

            try
            {
                using(StreamReader file = new StreamReader(@"characters/" + chrName))
                {
                    Character.clearCallNames();

                    while((line = await file.ReadLineAsync()) != null)
                    {
                        subLine = line.Split(':');
                        if(subLine.Length == 2)
                        {
                            try
                            {
                                Console.WriteLine(line);
                                switch(subLine[0])
                                {
                                    case "name":
                                        Character.addCallName(subLine[1]);
                                        name = subLine[1];
                                        break;

                                    case "picture":
                                        picture = subLine[1];
                                        break;

                                    case "game":
                                        game = subLine[1];
                                        break;

                                    case "altname":
                                        Character.addCallName(subLine[1]);
                                        break;

                                    case "role":
                                        roleName = subLine[1];
                                        break;

                                    default:
                                        Console.WriteLine("Invalid tag: " + subLine[0]);
                                        break;
                                }
                            }
                            catch(FileNotFoundException e)
                            {
                                Console.WriteLine("File not found");
                                Console.WriteLine(e.Message);
                            }
                            catch(RateLimitException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                        else if(line == "#Text")
                        {
                            break;
                        }
                        else
                            Console.WriteLine("Invalid line " + line);
                    }

                    await Character.updateText(file);
                }
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("File did not exist");
                Console.WriteLine(e.Message);
                await args.Channel.SendMessageAsync(Character.getText("nochr", chrName));
                return;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            await Program.update(name, picture, game, args.Guild, roleName);

            cooldown = DateTime.UtcNow.AddMinutes(20);
            Console.WriteLine(cooldown);

            await args.Channel.SendMessageAsync(Character.getText("loadedchr", chrName));
        }
    }
}
