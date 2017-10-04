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
        public static async Task runCommand(MessageCreateEventArgs args)
        {
            Console.WriteLine("Running Change Character command");

            string chrName = args.Message.Content.Substring(23);
            chrName = chrName.Remove(chrName.Length - 2);
            Console.WriteLine("Loading: " + chrName);
            await args.Channel.SendMessageAsync("Loading " + chrName + "...");

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
                    Program.clearCallNames();

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
                                        Program.addCallName(subLine[1]);
                                        name = subLine[1];
                                        break;

                                    case "picture":
                                        picture = subLine[1];
                                        break;

                                    case "game":
                                        game = subLine[1];
                                        break;

                                    case "altname":
                                        Program.addCallName(subLine[1]);
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
                        else
                            Console.WriteLine("Invalid line " + line);
                    }
                }
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("File did not exist");
                Console.WriteLine(e.Message);
                await args.Channel.SendMessageAsync(chrName + " does not exist.");
                return;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            await Program.update(name, picture, game, args.Guild, roleName);

            await args.Channel.SendMessageAsync(chrName + " loaded successfully.");
        }
    }
}
