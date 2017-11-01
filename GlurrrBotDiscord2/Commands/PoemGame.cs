using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2.Commands
{
    class PoemGame
    {
        const string SPACING = "                 ";
        static Random random;
        static DiscordMessage currentMessage;
        static bool running = false;
        static PoemWord[] currentWords;
        static int[] currentFavors;
        static int lastGirl;
        static int numRounds = 20;
        static int currentRound;

        public static async Task runCommand(MessageCreateEventArgs args)
        {
            string msg = args.Message.Content.ToLower();

            // Make sure Poem Game isn't currently running
            if(running)
            {
                Console.WriteLine("Poem Game is already running!");
            }

            // Should it be Quiz
            if(msg.Contains("quiz"))
            {
                Console.WriteLine("Running Poem Game Quiz");
                return;
            }
            // Poem Game instead
            else
            {
                Console.WriteLine("Writing a poem: " + PoemGameDictionary.getRandomWord());
                await args.Channel.SendMessageAsync(Character.getText("writepoem"));

                random = new Random();

                currentWords = new PoemWord[4];
                currentFavors = new int[3];
                numRounds = 20;
                currentRound = 1;
                lastGirl = 0;

                currentMessage = await args.Channel.SendMessageAsync(Character.getText("loadingpoemgame"));
                
                await currentMessage.CreateReactionAsync(DiscordEmoji.FromName(Program.discord, ":monika:"));
                await currentMessage.CreateReactionAsync(DiscordEmoji.FromName(Program.discord, ":sayori:"));
                await currentMessage.CreateReactionAsync(DiscordEmoji.FromName(Program.discord, ":natsuki:"));
                await currentMessage.CreateReactionAsync(DiscordEmoji.FromName(Program.discord, ":yuri:"));

                newWords();
                await buildMessage();

                running = true;
            }
        }

        public static async Task reactionAdded(MessageReactionAddEventArgs args)
        {
            if(!running || args.Message != currentMessage)
                return;

            Console.WriteLine("Interacted with Poem Game");
            if(args.Emoji.Name == "monika")
            {
                interactWord(0);
            }
            else if(args.Emoji.Name == "sayori")
            {
                interactWord(1);
            }
            else if(args.Emoji.Name == "natsuki")
            {
                interactWord(2);
            }
            else if(args.Emoji.Name == "yuri")
            {
                interactWord(3);
            }
            else
            {
                await args.Message.DeleteReactionAsync(args.Emoji, args.User, "Don't ruin the poem");
                return;
            }

            await args.Message.DeleteReactionAsync(args.Emoji, args.User);
        }

        static async Task buildMessage(string desc = "")
        {
            if(desc == "")
                desc = "|- " + currentWords[0].Word + " -|- " + currentWords[1].Word + " -|- " + currentWords[2].Word + " -|- " + currentWords[3].Word + " -|";

            if(lastGirl == 0)
                await currentMessage.ModifyAsync("**Doki Doki Poem Game**\n" + currentRound + "/20\n" + desc + "\n\n"
                    + "       "
                    + PoemGameDictionary.getEmoji(0)
                    + SPACING
                    + PoemGameDictionary.getEmoji(1)
                    + SPACING
                    + PoemGameDictionary.getEmoji(2)
                    + SPACING
                    + PoemGameDictionary.getEmoji(3));
            else if(lastGirl == 1)
                await currentMessage.ModifyAsync("**Doki Doki Poem Game**\n" + currentRound + "/20\n" + desc + "\n             " + SPACING + PoemGameDictionary.getEmoji(1) + "\n"
                    + "       "
                    + PoemGameDictionary.getEmoji(0)
                    + SPACING
                    + SPACING
                    + PoemGameDictionary.getEmoji(2)
                    + SPACING
                    + PoemGameDictionary.getEmoji(3));
            else if(lastGirl == 2)
                await currentMessage.ModifyAsync("**Doki Doki Poem Game**\n" + currentRound + "/20\n" + desc + "\n                   " + SPACING + SPACING + PoemGameDictionary.getEmoji(2) + "\n"
                    + "       "
                    + PoemGameDictionary.getEmoji(0)
                    + SPACING
                    + PoemGameDictionary.getEmoji(1)
                    + SPACING
                    + SPACING
                    + PoemGameDictionary.getEmoji(3));
            else if(lastGirl == 3)
                await currentMessage.ModifyAsync("**Doki Doki Poem Game**\n" + currentRound + "/20\n" + desc + "\n                         " + SPACING + SPACING + SPACING + PoemGameDictionary.getEmoji(3) + "\n"
                    + "       "
                    + PoemGameDictionary.getEmoji(0)
                    + SPACING
                    + PoemGameDictionary.getEmoji(1)
                    + SPACING
                    + PoemGameDictionary.getEmoji(2));


            //"      "
            //"    "
        }

        static void newWords()
        {
            // Options keeps track of which girl and the random word is still left
            // Choice is a holder for which option we're trying to do
            bool[] options = new bool[] { true, true, true, true };
            int choice;

            // Cycle 4 times for the three girl words and the random word
            for(int c = 0; c < 4; c++)
            {
                // Decide which choice
                choice = random.Next(0, 4);

                // Check to see if that option is already used
                if(options[choice] == false)
                {
                    // If it was, decide which way to move the number
                    // If it was at the bottom move it up until you find an empty one
                    if(choice == 0)
                    {
                        while(options[choice] == false)
                        {
                            choice++;
                        }
                    }

                    // If it was 1 or 2, randomly decide to move it up or down
                    // Search for empty, if it didn't find one, loop to the other side
                    else if(choice == 1)
                    {
                        if(random.Next(0, 2) == 0)
                        {
                            while(options[choice] == false)
                            {
                                choice--;
                                if(choice < 0)
                                    choice = options.Length - 1;
                            }
                        }
                        else
                        {
                            while(options[choice] == false)
                            {
                                choice++;
                                if(choice > options.Length - 1)
                                    choice = 0;
                            }
                        }
                    }

                    // Choice 2
                    else if(choice == 2)
                    {
                        if(random.Next(0, 2) == 0)
                        {
                            while(options[choice] == false)
                            {
                                choice--;
                                if(choice < 0)
                                    choice = options.Length - 1;
                            }
                        }
                        else
                        {
                            while(options[choice] == false)
                            {
                                choice++;
                                if(choice > options.Length - 1)
                                    choice = 0;
                            }
                        }
                    }

                    // If it was at the top, move it down
                    else if(choice == 3)
                    {
                        while(options[choice] == false)
                        {
                            choice--;
                        }
                    }
                }

                // If it's the random word, get from any girl. Otherwise get our choice.
                if(choice == 0)
                    currentWords[c] = PoemGameDictionary.getRandomWord();
                else
                    currentWords[c] = PoemGameDictionary.getRandomWord(choice);

                options[choice] = false;

                for(int x = 0; x < 4; x++)
                {
                    if(x != c)
                    {
                        if(currentWords[x] != null)
                            if(currentWords[c].Word == currentWords[x].Word)
                            {
                                options[choice] = true;
                                c--;
                                break;
                            }
                    }
                }
            }
        }

        static void interactWord(int word)
        {
            for(int c = 0; c < 3; c++)
            {
                currentFavors[c] += currentWords[word].getFavor(c);
            }
            
            if(currentWords[word].Sayori == 3)
            {
                lastGirl = 1;
            }
            else if(currentWords[word].Natsuki == 3)
            {
                lastGirl = 2;
            }
            else if(currentWords[word].Yuri == 3)
            {
                lastGirl = 3;
            }

            if(currentRound == numRounds)
            {
                endGame();
                return;
            }

            currentRound++;
            newWords();
            buildMessage();
        }

        static void endGame()
        {
            string builder = "";
            int highestFavor = 0;

            for(int c = 0; c < 3; c++)
            {
                if(currentFavors[c] > currentFavors[highestFavor])
                {
                    highestFavor = c;
                }
            }

            switch(highestFavor)
            {
                case 0:
                    builder += "You wrote a poem for Sayori!";
                    lastGirl = 1;
                    break;
                case 1:
                    builder += "You wrote a poem for Natsuki!";
                    lastGirl = 2;
                    break;
                case 2:
                    builder += "You wrote a poem for Yuri!";
                    lastGirl = 3;
                    break;
            }

            builder += " : ";
            for(int c = 0; c < 3; c++)
            {
                builder += currentFavors[c] + "-";
            }

            builder = builder.Remove(builder.Length - 1);
            buildMessage(builder);

            running = false;
        }
    }
}
