using DSharpPlus.EventArgs;
using HtmlAgilityPack;
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

            /*var html = @"https://safebooru.org/index.php?page=post&s=list";
            HtmlWeb web = new HtmlWeb();
            var htmldoc = web.Load(html);
            var node = htmldoc.DocumentNode.SelectNodes("//meta");

            foreach(var tag in node)
            {
                Console.WriteLine("Node Name: " + tag.Attributes["name"].Value);

            }*/
        }
    }
}
