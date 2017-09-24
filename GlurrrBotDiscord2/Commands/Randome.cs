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
            if(commandFound == false && (msg.Contains("add") || msg.Contains("追加")))
            {
                commandFound = true;

                // Check to make sure they put an object
                string[] splitString = args.Message.Content.Split('"');
                if(splitString.Length < 3)
                {
                    splitString = args.Message.Content.Split('”');
                    if(splitString.Length < 3)
                    {
                        if(!CommandHandler.japanMode)
                            await args.Message.Channel.SendMessageAsync("Enter something to add to the pool in quotes!");
                        else
                            await args.Message.Channel.SendMessageAsync("引用符で囲んでプールに追加するものを入力してください！");

                        return;
                    }
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
        // TODO: Save randome list by file by name, can retrieve by name
            if(commandFound == false && msg.Contains("save"))
            {
                commandFound = true;

                await saveList(args.Channel);
            }

        // Loads the Randome list from file
            if(commandFound == false && msg.Contains("load"))
            {
                commandFound = true;

                await loadList(args.Channel);
                await displayRandome(args.Channel);
            }

        // Removes a specified listing from either your own or the first one found
            if(commandFound == false && (msg.Contains("delete") || msg.Contains("削除")))
            {
                commandFound = true;

                // Check to make sure they put an object
                string[] splitString = args.Message.Content.Split('"');
                if(splitString.Length < 3)
                {
                    splitString = args.Message.Content.Split('”');
                    if(splitString.Length < 3)
                    {
                        if(!CommandHandler.japanMode)
                            await args.Message.Channel.SendMessageAsync("Enter something to add to the pool in quotes!");
                        else
                            await args.Message.Channel.SendMessageAsync("引用符で囲んでプールに追加するものを入力してください！");

                        return;
                    }
                }

                if(msg.Contains("from my") || msg.Contains("じぶんの"))
                {
                    // Look for the specified object from their own list
                    if(randomeList.ContainsKey(args.Author.Username))
                    {
                        if(randomeList[args.Author.Username].Contains(splitString[1]))
                        {
                            randomeList[args.Author.Username].Remove(splitString[1]);
                            Console.WriteLine("Deleted " + splitString[1] + " from " + args.Author.Username);
                            if(!CommandHandler.japanMode)
                                await args.Channel.SendMessageAsync("Deleted " + splitString[1] + " from " + args.Author.Username + "'s list");
                            else
                                await args.Channel.SendMessageAsync("削除済み " + splitString[1] + " から " + args.Author.Username + "〜の りすと");

                            return;
                        }
                        else
                        {
                            Console.WriteLine("Couldn't find " + splitString[1] + " on " + args.Author.Username + "'s list");
                            if(!CommandHandler.japanMode)
                                await args.Channel.SendMessageAsync("Couldn't find " + splitString[1] + " on " + args.Author.Username + "'s list");
                            else
                                await args.Channel.SendMessageAsync("見つかりませんでした " + splitString[1] + " に " + args.Author.Username + "〜の りすと");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Couldn't find " + args.Author.Username + "'s list");

                        if(!CommandHandler.japanMode)
                            await args.Channel.SendMessageAsync("Couldn't find " + args.Author.Username + "'s list");
                        else
                            await args.Channel.SendMessageAsync("見つかりませんでした " + args.Author.Username + "〜の りすと");
                    }
                }
                else
                {
                    // Look for the specified object in all lists
                    foreach(string i in randomeList.Keys)
                    {
                        if(randomeList[i].Contains(splitString[1]))
                        {
                            randomeList[i].Remove(splitString[1]);
                            Console.WriteLine("Deleted " + splitString[1] + " from " + i);
                            if(!CommandHandler.japanMode)
                                await args.Channel.SendMessageAsync("Deleted " + splitString[1] + " from " + i + "'s list");
                            else
                                await args.Channel.SendMessageAsync("削除済み " + splitString[1] + " から " + i + "〜の りすと");

                            return;
                        }
                    }

                    Console.WriteLine("Couldn't find " + splitString[1] + " anywhere");
                    if(!CommandHandler.japanMode)
                        await args.Channel.SendMessageAsync("Couldn't find " + splitString[1] + " anywhere");
                    else
                        await args.Channel.SendMessageAsync("見つかりませんでした " + splitString[1] + " どこでも");
                }
            }

        // Roll for randome
            if(commandFound == false && (msg.Contains("roll") || msg.Contains("ろる")))
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

                if(!CommandHandler.japanMode)
                    await args.Message.Channel.SendMessageAsync("Let's see...");
                else
                    await args.Message.Channel.SendMessageAsync("どれどれ...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                if(!CommandHandler.japanMode)
                    await args.Message.Channel.SendMessageAsync("And the winner is...");
                else
                    await args.Message.Channel.SendMessageAsync("そして勝者は...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);

                Random random = new Random();
                await args.Message.Channel.SendMessageAsync(rollOptions[random.Next(0, rollOptions.Count - 1)] + "!");
            }
            
        // Display the randome list
            if(commandFound == false && (msg.Contains("list") || msg.Contains("りすと")))
            {
                commandFound = true;
                await displayRandome(args.Message.Channel);
            }

        // Clear randome list
            if(commandFound == false && (msg.Contains("clear") || msg.Contains("くりあ")))
            {
                commandFound = true;
                randomeList.Clear();

                if(!CommandHandler.japanMode)
                    await args.Message.Channel.SendMessageAsync("Randome lists cleared");
                else
                    await args.Message.Channel.SendMessageAsync("ランドームリストがクリアされました");
            }
        }

        async static Task displayRandome(DiscordChannel channel)
        {
            if(randomeList.Keys.Count == 0)
            {
                Console.WriteLine("Empty Randome list");
                if(!CommandHandler.japanMode)
                    await channel.SendMessageAsync("Randome list is empty, nothing to display");
                else
                    await channel.SendMessageAsync("ランドームリストは空で、表示するものはありません");

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
                //builder += "**" + i + "** - {";
                foreach(string s in randomeList[i])
                {
                    builder += s + ", ";
                }
                builder = builder.Remove(builder.Length - 2);
                embed.AddField(i, builder, true);
                //builder += "}\n";
            }


            Console.WriteLine(builder);
            await channel.SendMessageAsync("", false, embed);
        }

        static async Task saveList(DiscordChannel channel)
        {
            Console.WriteLine("Saving Randome list");
            await channel.SendMessageAsync("Saving Randome lists...");

            using(StreamWriter file = new StreamWriter(@"randomelists\randomelist.txt"))
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

            Console.WriteLine("Finished saving Randome list");
            await channel.SendMessageAsync("Finished saving Randome lists");
        }

        static async Task loadList(DiscordChannel channel)
        {
            Console.WriteLine("Loading Randome list");
            await channel.SendMessageAsync("Loading Randome lists...");

            string line;
            string currentUser = "";

            randomeList.Clear();

            try
            {
                using(StreamReader file = new StreamReader(@"randomelists\randomelist.txt"))
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
            }

            Console.WriteLine("Finished loading Randome list");
            await channel.SendMessageAsync("Finished loading Randome lists");
        }
    }
}
