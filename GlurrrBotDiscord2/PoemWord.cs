using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2
{
    class PoemWord
    {
        string word;
        int sayori;
        int natsuki;
        int yuri;

        public PoemWord(string w, int s, int n, int y)
        {
            word = w;
            sayori = s;
            natsuki = n;
            yuri = y;
        }

        public int getFavor(int girl)
        {
            switch(girl)
            {
                case 0:
                    return sayori;
                case 1:
                    return natsuki;
                case 2:
                    return yuri;
            }

            return 0;
        }

        public string Word
        {
            get
            {
                return word;
            }
        }

        public int Sayori
        {
            get
            {
                return sayori;
            }
        }

        public int Natsuki
        {
            get
            {
                return natsuki;
            }

        }

        public int Yuri
        {
            get
            {
                return yuri;
            }
        }
    }
}
