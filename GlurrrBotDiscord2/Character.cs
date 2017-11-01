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

        public static string getText(string key, string format1 = "X", string format2 = "X")
        {
            if(text.ContainsKey(key))
            {
                return string.Format(text[key], format1, format2);
            }
            else
            {
                if(defaultText.ContainsKey(key))
                    return string.Format(defaultText[key], format1, format2);
                else
                    return getText("error");
            }
        }

        public static async Task loadDefault()
        {
            string line;
            string[] subline;

            using(StreamReader file = new StreamReader(@"characters/default.chr"))
            {
                Console.WriteLine("Loading default phrases");
                while((line = await file.ReadLineAsync()) != null)
                {
                    subline = line.Split('|');
                    if(subline.Length == 2)
                    {
                        Console.WriteLine("Loading default phrase: " + line);
                        defaultText.Add(subline[0], subline[1]);
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
                subline = line.Split('|');
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
