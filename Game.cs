using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace B23_Ex02
{
    class Game
    {
        private const byte k_FirstPlayerNumber = 1;
        private const byte k_SecondPlayerNumber = 2;
        private const byte k_NumberOfPlayersToChooseInRandom = 2;
        public const char k_FirstPlayerSign = 'X';
        public const char k_SecondPlayerSign = 'O';
        public const int k_MinimumSizeToBoard = 3;
        public const int k_MaximumSizeToBoard = 9;
        private const int k_InitialScoreForPlayers = 0;
        private readonly bool r_IsComputerPlaying;
        private BoardGame m_Board;
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private Random m_CompuerChoice;
        private eCurrentStatusOfTheGame m_StatusGame;
        
        public Game(bool i_IsPlayingAgainstComputer, string i_FirstPlayerName, string i_SecondPlayerName, byte i_BoardSize)
        {
            m_FirstPlayer = new Player(i_FirstPlayerName, k_FirstPlayerSign, ePlayerNumber.One, k_InitialScoreForPlayers);
            m_SecondPlayer = new Player(i_SecondPlayerName, k_SecondPlayerSign, ePlayerNumber.Two, k_InitialScoreForPlayers);
            m_Board = new BoardGame(i_BoardSize);
            r_IsComputerPlaying = i_IsPlayingAgainstComputer;
            m_StatusGame = eCurrentStatusOfTheGame.PlayingGame;
            m_CompuerChoice = new Random();
        }

        public Player PlayerOne
        {
            get { return m_FirstPlayer; }
        }

        public Player PlayerTwo
        {
            get { return m_SecondPlayer; }
        }

        public BoardGame Board
        {
            get { return m_Board; }
        }

        public bool IsComputerPlaying
        {
            get { return r_IsComputerPlaying; }
        }

        public eCurrentStatusOfTheGame StatusGame
        {
            get { return m_StatusGame; }
            set { m_StatusGame = value; }
        }

        public static bool IsTheBoardSizeInRange(int i_BoardSizeToCheck)
        {
            bool boardSizeIsOnRange = true;

            if (i_BoardSizeToCheck < k_MinimumSizeToBoard || i_BoardSizeToCheck > k_MaximumSizeToBoard)
            {
                boardSizeIsOnRange = false;
            }

            return boardSizeIsOnRange;
        }

        public void SwitchTurnBetweenPlayers(ref Player io_PlayerToSwitch)
        {
            if (io_PlayerToSwitch.Number == ePlayerNumber.One)
            {
                io_PlayerToSwitch = PlayerTwo;
            }
            else
            {
                io_PlayerToSwitch = PlayerOne;
            }
        }

        private void checkIfPlayerLooseByStrikeOnRow()
        {
            byte playerOneStrikeCounter = 1;
            byte playerTwoStrikeCounter = 1;

            for (int i = 0; i < Board.BoardSize; i++)
            {
                for (int j = 0; j < Board.BoardSize - 1; j++)
                {
                    if (Board[i, j].Sign == PlayerOne.Sign && Board[i, j].Sign == Board[i, j + 1].Sign)
                    {
                        playerOneStrikeCounter++;
                    }
                    else
                    {
                        playerOneStrikeCounter = 1;
                    }

                    if (Board[i, j].Sign == PlayerTwo.Sign && Board[i, j].Sign == Board[i, j + 1].Sign)
                    {
                        playerTwoStrikeCounter++;
                    }
                    else
                    {
                        playerTwoStrikeCounter = 1;
                    }

                    if (playerOneStrikeCounter == Board.BoardSize)
                    {
                        m_StatusGame = eCurrentStatusOfTheGame.PlayerOneLost;
                        PlayerTwo.Score++;
                    }

                    if (playerTwoStrikeCounter == Board.BoardSize)
                    {
                        m_StatusGame = eCurrentStatusOfTheGame.PlayerTwoLost;
                        PlayerOne.Score++;
                    }
                }
            }
        }

        private void checkIfPlayerLooseByDownDiagnol()
        {
            byte playerOneStrikeCounter = 1;
            byte playerTwoStrikeCounter = 1;

            for (int i = 0; i < Board.BoardSize - 1; i++)
            {
                if (Board[i, i].Sign == PlayerOne.Sign && Board[i, i].Sign == Board[i + 1, i + 1].Sign)
                {
                    playerOneStrikeCounter++;
                }
                else
                {
                    playerOneStrikeCounter = 1;
                }

                if (Board[i, i].Sign == PlayerTwo.Sign && Board[i, i].Sign == Board[i + 1, i + 1].Sign)
                {
                    playerTwoStrikeCounter++;
                }
                else
                {
                    playerTwoStrikeCounter = 1;
                }

                if (playerOneStrikeCounter == Board.BoardSize)
                {
                    m_StatusGame = eCurrentStatusOfTheGame.PlayerOneLost;
                    PlayerTwo.Score++;
                }

                if (playerTwoStrikeCounter == Board.BoardSize)
                {
                    m_StatusGame = eCurrentStatusOfTheGame.PlayerTwoLost;
                    PlayerOne.Score++;
                }
            }
        }

        private void checkIfPlayerLooseByUpDiagnol()
        {
            byte playerOneStrikeCounter = 1;
            byte playerTwoStrikeCounter = 1;
            int j = Board.BoardSize - 1;

            for (int i = 0; i < Board.BoardSize - 1 && j > 0; i++, j--)
            {
                if (Board[i, j].Sign == PlayerOne.Sign && Board[i, j].Sign == Board[i + 1, j - 1].Sign)
                {
                    playerOneStrikeCounter++;
                }
                else
                {
                    playerOneStrikeCounter = 1;
                }

                if (Board[i, j].Sign == PlayerTwo.Sign && Board[i, j].Sign == Board[i + 1, j - 1].Sign)
                {
                    playerTwoStrikeCounter++;
                }
                else
                {
                    playerTwoStrikeCounter = 1;
                }

                if (playerOneStrikeCounter == Board.BoardSize)
                {
                    m_StatusGame = eCurrentStatusOfTheGame.PlayerOneLost;
                    PlayerTwo.Score++;
                }

                if (playerTwoStrikeCounter == Board.BoardSize)
                {
                    m_StatusGame = eCurrentStatusOfTheGame.PlayerTwoLost;
                    PlayerOne.Score++;
                }
            }
        }

        private void checkIfPlayerLooseByStrikeOnCoulm()
        {
            byte playerOneStrikeCounter = 1;
            byte playerTwoStrikeCounter = 1;

            for (int i = 0; i < Board.BoardSize; i++)
            {
                for (int j = 0; j < Board.BoardSize - 1; j++)
                {
                    if (Board[j, i].Sign == PlayerOne.Sign && Board[j, i].Sign == Board[j + 1, i].Sign)
                    {
                        playerOneStrikeCounter++;
                    }
                    else
                    {
                        playerOneStrikeCounter = 1;
                    }

                    if (Board[j, i].Sign == PlayerTwo.Sign && Board[j, i].Sign == Board[j + 1, i].Sign)
                    {
                        playerTwoStrikeCounter++;
                    }
                    else
                    {
                        playerTwoStrikeCounter = 1;
                    }

                    if (playerOneStrikeCounter == Board.BoardSize)
                    {
                        m_StatusGame = eCurrentStatusOfTheGame.PlayerOneLost;
                        PlayerTwo.Score++;
                    }

                    if (playerTwoStrikeCounter == Board.BoardSize)
                    {
                        m_StatusGame = eCurrentStatusOfTheGame.PlayerTwoLost;
                        PlayerOne.Score++;
                    }
                }
            }
        }

        public void CheckIfTheTurnFinishedWithTie()
        {
            if (m_Board.AmountOfFullCells == Math.Pow(m_Board.BoardSize, 2))
            {
                m_StatusGame = eCurrentStatusOfTheGame.ItIsTie;
            }
        }

        public void InitializationTheBoardForNewRound(byte i_BoardSize)
        {
            m_Board = new BoardGame(i_BoardSize);
            m_StatusGame = eCurrentStatusOfTheGame.PlayingGame;
        }

        public  void GetCoordinateFromComputer(out int o_Row, out int o_Col)
        {
            o_Row = m_CompuerChoice.Next(1, m_Board.BoardSize + 1);
            o_Col = m_CompuerChoice.Next(1, m_Board.BoardSize + 1);
            while(m_Board[o_Row-1, o_Col-1].TheCellIsAlredyMarked() == true)
            {
                o_Row = m_CompuerChoice.Next(1, m_Board.BoardSize + 1);
                o_Col = m_CompuerChoice.Next(1, m_Board.BoardSize + 1);
            }

            Thread.Sleep(1000);
        }

        public void UpdateScoreAfterOneOfThePlayersQuit()
        {
            if(StatusGame == eCurrentStatusOfTheGame.PlayerOneChoseToQuitTheGame)
            {
                PlayerTwo.Score++;
            }

            if(StatusGame == eCurrentStatusOfTheGame.PlayerTwoChoseToQuitTheGame)
            {
                PlayerOne.Score++;
            }
        }

        public void CheckIfOneOfThePlayersLoose()
        {
            checkIfPlayerLooseByStrikeOnCoulm();
            checkIfPlayerLooseByStrikeOnRow();
            checkIfPlayerLooseByDownDiagnol();
            checkIfPlayerLooseByUpDiagnol();
        }

        public  void GetThePlayerWhoPlayFirstInRandomWay(out Player o_PlayerToChooseForFirstTurn)
        {
            Random chooseRandomPlayer = new Random();
            int lottery = chooseRandomPlayer.Next(k_NumberOfPlayersToChooseInRandom);

            if (lottery == k_FirstPlayerNumber)
            {
                o_PlayerToChooseForFirstTurn = m_FirstPlayer;
            }
            else
            {
                o_PlayerToChooseForFirstTurn = m_SecondPlayer;
            }
        }
    }
}
