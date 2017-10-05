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
        public static List<string> callNames = new List<string>();

        static Dictionary<string, string> defaultText = new Dictionary<string, string>();
        static Dictionary<string, string> text = new Dictionary<string, string>();
        //static Dictionary<string, string> consoleText;

        public static string getText(string key, string format1 = null, string format2 = null)
        {
            if(text.ContainsKey(key))
            {
                return string.Format(text[key], format1);
            }
            else
            {
                return string.Format(defaultText[key], format1, format2);
            }
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

        public static void addCallName(string name)
        {
            if(!(callNames.Contains(name)))
                callNames.Add(name.ToLower());
        }


        public static void clearCallNames()
        {
            callNames.Clear();
        }
    }
}
