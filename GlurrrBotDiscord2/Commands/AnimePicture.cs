using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2.Commands
{
    public class AnimePicture
    {
        public static async Task runCommand(MessageCreateEventArgs args)
        {
            Random random = new Random();
            await args.Channel.SendMessageAsync("http://danbooru.donmai.us/posts/" + random.Next(1, 2823187));
        }
    }
}
