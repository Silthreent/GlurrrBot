using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
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

            // Add to Randome list
            if(commandFound == false && args.Message.Content.ToLower().Contains("add"))
            {
                commandFound = true;

                string[] splitString = args.Message.Content.Split('"');
                if(splitString.Length < 3)
                {
                    Console.WriteLine("NO QUOTES ENTERED");
                    await args.Message.Channel.SendMessageAsync("Enter something to add to the pool in quotes!");
                    return;
                }

                if(randomeList.ContainsKey(args.Author.Username))
                {
                    Console.WriteLine("Author entry existed");
                    randomeList[args.Message.Author.Username].Add(splitString[1]);
                }
                else
                {
                    Console.WriteLine("Author entry did not exist");
                    randomeList.Add(args.Message.Author.Username, new List<string>());
                    randomeList[args.Message.Author.Username].Add(splitString[1]);
                }

                await displayRandome(args.Message.Channel);
                //saveJSON();
            }

            // Roll for randome
            if(commandFound == false && args.Message.Content.ToLower().Contains("roll"))
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

                await args.Message.Channel.SendMessageAsync("Let's see...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("And the winner is...");
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

            // Clear randome list
            if(commandFound == false && args.Message.Content.ToLower().Contains("clear"))
            {
                commandFound = true;
                randomeList.Clear();

                await args.Message.Channel.SendMessageAsync("Randome lists clears");
            }

            // Display the randome list
            if(commandFound == false && args.Message.Content.ToLower().Contains("list"))
            {
                commandFound = true;
                await displayRandome(args.Message.Channel);
            }
        }

        async static Task displayRandome(DiscordChannel channel)
        {
            if(randomeList.Keys.Count == 0)
            {
                await channel.SendMessageAsync("Randome list is empty, nothing to display");
                return;
            }

            Console.WriteLine("Displaying results");
            string builder = "";
            foreach(string i in randomeList.Keys)
            {
                builder += i + " - {";
                foreach(string s in randomeList[i])
                {
                    builder += s + ", ";
                }
                builder = builder.Remove(builder.Length - 2);
                builder += "}\n";
            }

            Console.WriteLine(builder);
            await channel.SendMessageAsync(builder);
        }
    }
}
