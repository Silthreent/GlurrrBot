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
    public class Randome
    {
        static Dictionary<string, List<string>> randomeList = new Dictionary<string, List<string>>();

        public static async Task runCommand(MessageCreateEventArgs args)
        {
            bool commandFound = false;
            string msg = args.Message.Content.ToLower();

        // Add to Randome list
            if(commandFound == false && msg.Contains("add"))
            {
                commandFound = true;

                // Check to make sure they put an object
                string[] splitString = args.Message.Content.Split('"');
                if(splitString.Length < 3)
                {
                    await args.Message.Channel.SendMessageAsync(Character.getText("noquotes"));
                    return;
                }

                // Add it to their list, if they don't have a list yet create them one first
                if(!randomeList.ContainsKey(args.Author.Username))
                {
                    Console.WriteLine("Author entry did not exist");
                    randomeList.Add(args.Author.Username, new List<string>());
                }

                randomeList[args.Author.Username].Add(splitString[1].ToLower());


                await displayRandome(args.Message.Channel);
            }

        // Saves the Randome list to file to retrieve later
            if(commandFound == false && msg.Contains("save"))
            {
                commandFound = true;

                //Check if they put a name in quotes
                // If they did, save that. Otherwise default.
                string[] splitString = msg.Split('"');
                if(splitString.Length < 3)
                {
                    Console.WriteLine("No name entered, saving to default");
                    await args.Channel.SendMessageAsync(Character.getText("saveefault"));
                    await saveList(args.Channel);
                    return;
                }

                Console.WriteLine("Saving list to " + splitString[1]);
                await saveList(args.Channel, splitString[1]);
            }

        // Loads the Randome list from file
            if(commandFound == false && msg.Contains("load"))
            {
                commandFound = true;

                //Check if they put a name in quotes
                // If they did, load that. Otherwise default.
                string[] splitString = msg.Split('"');
                if(splitString.Length < 3)
                {
                    Console.WriteLine("No name entered, loading default");
                    await args.Channel.SendMessageAsync(Character.getText("loaddefault"));
                    await loadList(args.Channel);
                    await displayRandome(args.Channel);
                    return;
                }

                Console.WriteLine("Loading " + splitString[1]);
                await loadList(args.Channel, splitString[1]);
                await displayRandome(args.Channel);
            }

        // Removes a specified listing from either your own or the first one found
            if(commandFound == false && msg.Contains("delete"))
            {
                commandFound = true;

                // Check to make sure they put an object
                string[] splitString = args.Message.Content.Split('"');
                if(splitString.Length < 3)
                {
                    await args.Message.Channel.SendMessageAsync("noquotes");
                    return;
                }

                // Delete an entire Randome list file
                if(msg.Contains("list"))
                {
                    File.Delete(@"randomelists/" + splitString[1] + ".txt");
                    Console.WriteLine("Randome list " + splitString[1]);
                    await args.Channel.SendMessageAsync(Character.getText("deletelist", splitString[1]));
                    return;
                }

                // Delete a object from a list
                if(msg.Contains("from my"))
                {
                    // Look for the specified object from their own list
                    if(randomeList.ContainsKey(args.Author.Username))
                    {
                        if(randomeList[args.Author.Username].Contains(splitString[1]))
                        {
                            randomeList[args.Author.Username].Remove(splitString[1]);
                            Console.WriteLine("Deleted " + splitString[1] + " from " + args.Author.Username);
                            await args.Channel.SendMessageAsync(Character.getText("deletefrom", splitString[1], args.Author.Username));
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Couldn't find " + splitString[1] + " on " + args.Author.Username + "'s list");
                            await args.Channel.SendMessageAsync(Character.getText("couldntfind", splitString[1]));
                        }
                    }
                    else
                    {
                        Console.WriteLine("Couldn't find " + args.Author.Username + "'s list");

                        await args.Channel.SendMessageAsync(Character.getText("couldntfind", args.Author.Username + "'s Randome"));
                    }
                }
                // Delete the object from any list
                else
                {
                    // Look for the specified object in all lists
                    foreach(string i in randomeList.Keys)
                    {
                        if(randomeList[i].Contains(splitString[1]))
                        {
                            randomeList[i].Remove(splitString[1]);
                            Console.WriteLine("Deleted " + splitString[1] + " from " + i);
                            await args.Channel.SendMessageAsync(Character.getText("deletefrom", splitString[1], i));

                            return;
                        }
                    }

                    Console.WriteLine("Couldn't find " + splitString[1] + " anywhere");
                    await args.Channel.SendMessageAsync(Character.getText("couldntfind", splitString[1]));
                }
            }

        // Roll for randome
            if(commandFound == false && msg.Contains("roll"))
            {
                commandFound = true;

                List<string> rollOptions = new List<string>();

                foreach(string i in randomeList.Keys)
                {
                    foreach(string s in randomeList[i])
                    {
                        rollOptions.Add(i + "'s choice of " + s);
                    }
                }

                if(rollOptions.Count == 0)
                {
                    Console.WriteLine("Randome list is empty; can't roll");
                    await args.Channel.SendMessageAsync(Character.getText("rollfail"));
                    return;
                }

                await args.Message.Channel.SendMessageAsync(Character.getText("roll1"));
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync(Character.getText("roll2"));
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync(Character.getText("roll3"));
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync(Character.getText("roll4"));
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);

                Random random = new Random();
                await args.Message.Channel.SendMessageAsync(Character.getText("rollwinner", rollOptions[random.Next(0, rollOptions.Count - 1)]));
            }
            
        // Display the randome list
            if(commandFound == false && msg.Contains("list"))
            {
                commandFound = true;
                await displayRandome(args.Message.Channel);
            }

        // Clear randome list
            if(commandFound == false && msg.Contains("clear"))
            {
                commandFound = true;
                randomeList.Clear();

                await args.Message.Channel.SendMessageAsync(Character.getText("randomeclear"));
            }
        }

        // Method used to display the currently loaded Randome list
        async static Task displayRandome(DiscordChannel channel)
        {
            if(randomeList.Keys.Count == 0)
            {
                Console.WriteLine("Empty Randome list");
                await channel.SendMessageAsync(Character.getText("randomedisplayfail"));

                return;
            }

            Console.WriteLine("Displaying results");
            string builder = "";

            var embed = new DiscordEmbedBuilder()
            {
                Title = "Randome List",
                Color = DiscordColor.Violet,
            };

            foreach(string i in randomeList.Keys)
            {
                foreach(string s in randomeList[i])
                {
                    builder += s + ", ";
                }
                builder = builder.Remove(builder.Length - 2);
                embed.AddField(i, builder, true);
                builder = "";
            }

            Console.WriteLine(builder);
            await channel.SendMessageAsync("", false, embed);
        }

        // Save the current Randome list, either to default or to the entered name
        static async Task saveList(DiscordChannel channel, string fileName = "randomelist")
        {
            Console.WriteLine("Saving Randome list");
            await channel.SendMessageAsync(Character.getText("savinglist"));

            using(StreamWriter file = new StreamWriter(@"randomelists/" + fileName + ".txt"))
            {
                foreach(string i in randomeList.Keys)
                {
                    await file.WriteLineAsync("#" + i);
                    foreach(string c in randomeList[i])
                    {
                        await file.WriteLineAsync(c);
                    }
                }
            }

            Console.WriteLine("Finished saving Randome list " + fileName);
            await channel.SendMessageAsync(Character.getText("finishsave", fileName));
        }

        // Load a Randome list, either the default or the one entered
        static async Task loadList(DiscordChannel channel, string fileName = "randomelist")
        {
            Console.WriteLine("Loading Randome list " + fileName);
            await channel.SendMessageAsync(Character.getText("loadinglist", fileName));

            string line;
            string currentUser = "";

            randomeList.Clear();

            try
            {
                using(StreamReader file = new StreamReader(@"randomelists/" + fileName + ".txt"))
                {
                    while((line = await file.ReadLineAsync()) != null)
                    {
                        if(line[0] == '#')
                        {
                            currentUser = line.Substring(1);
                            if(!randomeList.ContainsKey(currentUser))
                            {
                                Console.WriteLine("Author entry did not exist");
                                randomeList.Add(currentUser, new List<string>());
                            }
                        }
                        else
                        {
                            randomeList[currentUser].Add(line);
                        }
                    }
                }
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("File did not exist");
                Console.WriteLine(e.Message);
                await channel.SendMessageAsync(Character.getText("couldntfind", fileName));
            }

            Console.WriteLine("Finished loading Randome list");
            await channel.SendMessageAsync(Character.getText("finishedload"));
        }
    }
}
