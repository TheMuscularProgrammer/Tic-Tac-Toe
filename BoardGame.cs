using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B23_Ex02
{
    class BoardGame
    {
        private readonly BoardCell[,] r_BoardGame;
        private byte m_BoardSize;
        private byte m_AmountOfFullCells;
        private const char k_Blank = ' ';

        public BoardGame(byte i_BoardSize)
        {
            m_BoardSize = i_BoardSize;
            m_AmountOfFullCells = 0;
            r_BoardGame = new BoardCell[i_BoardSize, i_BoardSize];
        }
        
        public BoardCell this[int row, int col]
        {
            get { return r_BoardGame[row, col]; }
            set { r_BoardGame[row, col] = value; }
        }

        public byte BoardSize
        {
            get { return m_BoardSize; }
            set { m_BoardSize = value; }
        }

        public byte AmountOfFullCells
        {
            get { return m_AmountOfFullCells; }
        }

        public void UpdateBoard(int i_RowToUpdate, int i_ColToUpdate, char i_SignOfPlayerWhoMarkTheCell)
        {
            if (i_RowToUpdate > 0 && i_ColToUpdate > 0)
            {
                r_BoardGame[i_RowToUpdate - 1, i_ColToUpdate - 1].Sign = i_SignOfPlayerWhoMarkTheCell;
                r_BoardGame[i_RowToUpdate - 1, i_ColToUpdate - 1].isMark = true;
                m_AmountOfFullCells++;
            }
        }

        public struct BoardCell
        {
            private byte m_Row;
            private byte m_Col;
            private bool m_IsMark;
            private char m_Sign;

            public BoardCell(byte i_Row, byte i_Col)
            {
                m_Row = i_Row;
                m_Col = i_Col;
                m_IsMark = false;
                m_Sign = k_Blank;
            }

            public char Sign
            {
                get { return m_Sign; }
                set { m_Sign = value; }
            }

            public bool isMark
            {
                get { return m_IsMark; }
                set { m_IsMark = value; }
            }

            public byte Row
            {
                get { return m_Row; }
                set { m_Row = value; }
            }

            public byte Col
            {
                get { return m_Col; }
                set { m_Col = value; }
            }

            public bool TheCellIsAlredyMarked()
            {
                return isMark;
            }
        }
    }
}
