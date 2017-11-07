using DSharpPlus.Entities;
using GlurrrBotDiscord2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2
{
    class PoemGameDictionary
    {
        const string MONIKA = "Monika";
        static Random random;
        static List<PoemWord> sWords;
        static List<PoemWord> nWords;
        static List<PoemWord> yWords;

        public static void buildDictionary()
        {
            random = new Random();

            sWords = new List<PoemWord>();
            nWords = new List<PoemWord>();
            yWords = new List<PoemWord>();

            // Sayori
            sWords.Add(new PoemWord("adventure", 3, 2, 1));
            sWords.Add(new PoemWord(" amazing ", 3, 2, 1));
            sWords.Add(new PoemWord(" beauty  ", 3, 2, 1));
            sWords.Add(new PoemWord("   bed   ", 3, 2, 1));
            sWords.Add(new PoemWord("  bliss  ", 3, 2, 1));
            sWords.Add(new PoemWord("  alone  ", 3, 1, 2));
            sWords.Add(new PoemWord("  broken ", 3, 1, 2));
            sWords.Add(new PoemWord("  calm   ", 3, 1, 2));
            sWords.Add(new PoemWord("  alone  ", 3, 1, 2));
            sWords.Add(new PoemWord("   cry   ", 3, 1, 2));

            // Natsuki
            nWords.Add(new PoemWord("  anger  ", 2, 3, 1));
            nWords.Add(new PoemWord("  anime  ", 2, 3, 1));
            nWords.Add(new PoemWord("  boop   ", 2, 3, 1));
            nWords.Add(new PoemWord("  bouncy ", 2, 3, 1));
            nWords.Add(new PoemWord(" bubbles ", 2, 3, 1));
            nWords.Add(new PoemWord(" blanket ", 1, 3, 2));
            nWords.Add(new PoemWord(" fantasy ", 1, 3, 2));
            nWords.Add(new PoemWord("heartbeat", 1, 3, 2));
            nWords.Add(new PoemWord("  kiss   ", 1, 3, 2));
            nWords.Add(new PoemWord("lipstick ", 1, 3, 2));

            // Yuri
            yWords.Add(new PoemWord(" breathe ", 1, 2, 3));
            yWords.Add(new PoemWord("  cage   ", 1, 2, 3));
            yWords.Add(new PoemWord(" climax  ", 1, 2, 3));
            yWords.Add(new PoemWord(" desire  ", 1, 2, 3));
            yWords.Add(new PoemWord(" explode ", 1, 2, 3));
            yWords.Add(new PoemWord("agonizing", 2, 1, 3));
            yWords.Add(new PoemWord(" ambient ", 2, 1, 3));
            yWords.Add(new PoemWord(" anxiety ", 2, 1, 3));
            yWords.Add(new PoemWord("  atone  ", 2, 1, 3));
            yWords.Add(new PoemWord("  aura   ", 2, 1, 3));

            yWords.Add(new PoemWord("  dream  ", 2, 2, 3));
        }

        public static PoemWord getRandomWord(int girl = 0)
        {
            if(PoemGame.MonikaMode)
            {
                string builder = "";
                for(int c = 0; c < MONIKA.Length; c++)
                {
                    if(random.Next(0, 3) == 0)
                    {
                        builder += " ";
                    }
                    else
                    {
                        builder += MONIKA[c];
                    }
                }

                return new PoemWord(builder, 0, 0, 0);
            }

            if(girl == 0)
            {
                switch(random.Next(1, 4))
                {
                    case 1:
                        return sWords.ElementAt(random.Next(0, sWords.Count));
                    case 2:
                        return nWords.ElementAt(random.Next(0, nWords.Count));
                    case 3:
                        return yWords.ElementAt(random.Next(0, yWords.Count));
                }
            }
            else
            {
                switch(girl)
                {
                    case 1:
                    return sWords.ElementAt(random.Next(0, sWords.Count));
                    case 2:
                    return nWords.ElementAt(random.Next(0, nWords.Count));
                    case 3:
                    return yWords.ElementAt(random.Next(0, yWords.Count));
                }
            }

            return new PoemWord("error", 0, 0, 0);
        }

        public static PoemWord getWord(string word)
        {
            foreach(PoemWord x in sWords)
            {
                if(x.Word.Contains(word))
                {
                    return x;
                }
            }
            foreach(PoemWord x in nWords)
            {
                if(x.Word.Contains(word))
                {
                    return x;
                }
            }
            foreach(PoemWord x in yWords)
            {
                if(x.Word.Contains(word))
                {
                    return x;
                }
            }

            return new PoemWord("error", 0, 0, 0);
        }

        public static DiscordEmoji getEmoji(int girl)
        {
            switch(girl)
            {
                case 0:
                    return DiscordEmoji.FromName(Program.discord, ":monika:");
                case 1:
                    return DiscordEmoji.FromName(Program.discord, ":sayori:");
                case 2:
                    return DiscordEmoji.FromName(Program.discord, ":natsuki:");
                case 3:
                    if(random.Next(0, 100) == 0)
                        return DiscordEmoji.FromName(Program.discord, ":yurispook:");
                    else
                        return DiscordEmoji.FromName(Program.discord, ":yuri:");
            }

            return DiscordEmoji.FromName(Program.discord, ":thinking:");
        }

    }
}
