using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2.Commands
{
    public class WelcomeMessage
    {
        public static async Task welcomeMessage(PresenceUpdateEventArgs args)
        {
            if(File.Exists(@"welcomemessages/" + args.Member.Id + ".txt"))
            {
                Console.WriteLine("Welcome message found");
                using(StreamReader file = new StreamReader(@"welcomemessages/" + args.Member.Id + ".txt"))
                {
                    Console.WriteLine(args.Member.Username + " has a welcome message");
                    await args.Guild.Channels[0].SendMessageAsync(file.ReadLineAsync().Result);
                }
            }
        }

        public static async Task updateMessages(MessageCreateEventArgs args)
        {
            Console.WriteLine("Checking Welcome Messages command");

        //Set the welcome message. Attach to the pinged user to the message in quotes
            if(args.Message.Content.ToLower().Contains("set"))
            {
                Console.WriteLine("Setting welcome messsage");
                if(args.MentionedUsers.Count == 0)
                {
                    Console.WriteLine("No user to attach to");
                    await args.Channel.SendMessageAsync("No user to put message with");
                    return;
                }

                string[] quoteSplit = args.Message.Content.Split('"');
                if(quoteSplit.Count() < 3)
                {
                    Console.WriteLine("No message to attach");
                    await args.Channel.SendMessageAsync("Enter a message in quotes");
                    return;
                }

                using(StreamWriter file = new StreamWriter(@"welcomemessages/" + args.MentionedUsers[0].Id + ".txt"))
                {
                    await file.WriteAsync(quoteSplit[1]);
                    Console.WriteLine("Set " + args.MentionedUsers[0].Username + "'s welcome message to " + quoteSplit[1]);
                    await args.Channel.SendMessageAsync("Set " + args.MentionedUsers[0].Username + "'s welcome message to " + quoteSplit[1]);
                }
            }

        // Remove someone's welcome message
            if(args.Message.Content.ToLower().Contains("remove"))
            {
                Console.WriteLine("Removing welcome message");
                if(args.MentionedUsers.Count == 0)
                {
                    Console.WriteLine("No user to remove message");
                    await args.Channel.SendMessageAsync("No user to put message with");
                    return;
                }

                File.Delete(@"welcomemessages/" + args.MentionedUsers[0].Id + ".txt");

                Console.WriteLine("Welcome message for " + args.MentionedUsers[0].Username + " removed");
                await args.Channel.SendMessageAsync("Welcome message for " + args.MentionedUsers[0].Username + " removed");
            }
        }
    }
}
