using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B23_Ex02
{
    class Player
    {
        private readonly char r_Sign;
        private readonly string r_Name;
        private short m_Score;
        ePlayerNumber m_PlayerNumber;

        public Player(string i_PlayerName, char i_Sign , ePlayerNumber i_Number, short i_Score)
        {
            r_Name = i_PlayerName;
            r_Sign = i_Sign;
            m_PlayerNumber = i_Number;
            m_Score = i_Score;
        }

        public string Name
        {
            get { return r_Name; }
        }

        public short Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public char Sign
        {
            get { return r_Sign; }
        }

        public ePlayerNumber Number
        {
            get { return m_PlayerNumber; }
            set { m_PlayerNumber = value; }
        }
    }
}

