using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2.Commands
{
    public class KeepYourselfSafe
    {
        public static async Task runCommand(MessageCreateEventArgs args)
        {
            try
            {
                DiscordMessage msg = null;
                var msgs = await args.Channel.GetMessagesAsync(5);

                if(args.Message.MentionedUsers.Count == 1)
                {
                    foreach(DiscordMessage i in msgs)
                    {
                        if(i.Author == args.Message.MentionedUsers[0])
                        {
                            Console.WriteLine("Telling " + args.Message.MentionedUsers[0].Username + " to keep themself safe");
                            msg = i;
                            break;
                        }
                    }
                }
                else
                {
                    msg = msgs[1];
                }
                Console.WriteLine("Found message: " + msg.Content);
                await msg.CreateReactionAsync(DiscordEmoji.FromName(Program.discord, ":regional_indicator_k:"));
                await msg.CreateReactionAsync(DiscordEmoji.FromName(Program.discord, ":regional_indicator_y:"));
                await msg.CreateReactionAsync(DiscordEmoji.FromName(Program.discord, ":regional_indicator_s:"));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
