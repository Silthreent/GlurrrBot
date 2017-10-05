using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2
{
    public class Character
    {
        static Dictionary<string, string> defaultText = new Dictionary<string, string>();
        static Dictionary<string, string> text = new Dictionary<string, string>();
        static Dictionary<string, string> consoleText;

        public static string getText(string key)
        {
            return text[key];
        }

        public static async Task loadDefault()
        {
            string line;
            string[] subline;

            using(StreamReader file = new StreamReader(@"characters/default.chr"))
            {
                while((line = await file.ReadLineAsync()) != null)
                {
                    subline = line.Split(':');
                    if(subline.Length == 2)
                    {
                        text.Add(subline[0], subline[1]);
                    }
                }
            }
        }

        public static async Task updateText(StreamReader file)
        {
            text.Clear();

            string line;
            string[] subline;

            while((line = await file.ReadLineAsync()) != null)
            {
                subline = line.Split(':');
                text[subline[0]] = subline[1];
            }
        }
    }
}
