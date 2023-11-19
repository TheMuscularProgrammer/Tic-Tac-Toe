using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B23_Ex02
{
    class UserInterface
    {
        private const char k_QuitGameSign = 'Q';
        private const byte k_FirstPlayerNumber = 1;
        private const byte k_SecondPlayerNumber = 2;
        private const int k_IrrelevantVariable = -1;
        private static Game s_Game;

        public static void StartToPlayXMixDrixGame()
        {
            buildGame();
            printBoard();
            playGameLogicMoves();
        }

        private static void playGameLogicMoves()
        {
            Player currentPlayer;
            int rowThatThePlayerNeedToChoose;
            int columThatThePlayerNeedToChoose;

            s_Game.GetThePlayerWhoPlayFirstInRandomWay(out currentPlayer);
            while (s_Game.StatusGame == eCurrentStatusOfTheGame.PlayingGame)
            {
                playCurrentPlayerTurn(ref currentPlayer, out rowThatThePlayerNeedToChoose, out columThatThePlayerNeedToChoose);
            }

            displayToUserTheCurrentStatusOfGame(s_Game.StatusGame);
            checkIfUserChooseToPlayOntherRound();
        }

        private static void playCurrentPlayerTurn(ref Player io_CurrentPlayer, out int o_RowToChooseAndUpdateBoardForCurrentTurn, out int o_ColumToChooseAndUpdateBoardForCurrentTurn)
        {
            string msg;

            msg = string.Format($"Now its {io_CurrentPlayer.Name} turn! {Environment.NewLine}");
            Console.WriteLine(msg);
            getCoordinateFromCurrentPlayer(ref io_CurrentPlayer, out o_RowToChooseAndUpdateBoardForCurrentTurn, out o_ColumToChooseAndUpdateBoardForCurrentTurn);
            makeSureThatCurrentPlayerInsertCoordinateNotToMarkedCellOnBoard(ref o_RowToChooseAndUpdateBoardForCurrentTurn, ref o_ColumToChooseAndUpdateBoardForCurrentTurn, ref io_CurrentPlayer);
            s_Game.Board.UpdateBoard(o_RowToChooseAndUpdateBoardForCurrentTurn, o_ColumToChooseAndUpdateBoardForCurrentTurn, io_CurrentPlayer.Sign);
            checkIfTheTurnFinishWithDecisionAndUpdateScoreAndStatusOfGameInAccordance();
            s_Game.SwitchTurnBetweenPlayers(ref io_CurrentPlayer);
            Ex02.ConsoleUtils.Screen.Clear();
            printBoard();
        }

        private static void checkIfTheTurnFinishWithDecisionAndUpdateScoreAndStatusOfGameInAccordance()
        {
            s_Game.CheckIfTheTurnFinishedWithTie();
            s_Game.CheckIfOneOfThePlayersLoose();
            s_Game.UpdateScoreAfterOneOfThePlayersQuit();
        }

        private static void scoreUpdateMessage()
        {
            Console.WriteLine($"The Score is {s_Game.PlayerOne.Score} points to {s_Game.PlayerOne.Name} and {s_Game.PlayerTwo.Score} points to {s_Game.PlayerTwo.Name}!");
        }

        private static void displayToUserTheCurrentStatusOfGame(eCurrentStatusOfTheGame i_StatusOfGame)
        {
            switch(i_StatusOfGame)
            {
                case eCurrentStatusOfTheGame.PlayerOneLost:
                    {
                        Console.WriteLine($"{s_Game.PlayerOne.Name} Lost! ");
                        scoreUpdateMessage();
                        break;
                    }
                case eCurrentStatusOfTheGame.PlayerTwoLost:
                    {
                        Console.WriteLine($"{s_Game.PlayerTwo.Name} Lost! ");
                        scoreUpdateMessage();
                        break;
                    }
                case eCurrentStatusOfTheGame.ItIsTie:
                    {
                        Console.WriteLine("Its a Tie! ");
                        scoreUpdateMessage();
                        break;
                    }
                case eCurrentStatusOfTheGame.PlayerOneChoseToQuitTheGame:
                    {
                        Console.WriteLine($"{s_Game.PlayerOne.Name} Quit From Game!");
                        scoreUpdateMessage();
                        break;
                    }
                case eCurrentStatusOfTheGame.PlayerTwoChoseToQuitTheGame:
                    {
                        Console.WriteLine($"{s_Game.PlayerTwo.Name} Quit From Game!");
                        scoreUpdateMessage();
                        break;
                    }
            }    
        }

        private static void makeSureThatCurrentPlayerInsertCoordinateNotToMarkedCellOnBoard(ref int io_Row, ref int io_Col, ref Player io_CurrenttPlayer)
        {
            if (s_Game.StatusGame == eCurrentStatusOfTheGame.PlayingGame)
            {
                while (s_Game.Board[io_Row - 1, io_Col - 1].TheCellIsAlredyMarked())
                {
                    Console.WriteLine("this cell is alredy marked! please enter new cell!");
                    getCoordinateFromUser(out io_Row, out io_Col, ref io_CurrenttPlayer);
                }
            }
        }

        private static void getCoordinateFromUser(out int o_Row, out int o_Col, ref Player io_CurrentPlayer)
        {
            Console.WriteLine("please enter a row you want you sign or Q to quit from game");
            getDataFromUser(out o_Row, ref io_CurrentPlayer);
            if (s_Game.StatusGame == eCurrentStatusOfTheGame.PlayingGame)
            {
                Console.WriteLine("please enter a col you want to sign or Q to quit from game");
                getDataFromUser(out o_Col, ref io_CurrentPlayer);
            }
            else
            {
                o_Col = k_IrrelevantVariable;
            }
        }

        private static void getDataFromUser(out int o_DataFromUser, ref Player io_CurrentPlayer)
        {
            string inputFromUserToCheckValidation = Console.ReadLine();

            while(userChooseEnterInsteadNumber(inputFromUserToCheckValidation) == true)
            {
                Console.WriteLine("invalid input. please choose valid input.");
                inputFromUserToCheckValidation = Console.ReadLine();
                makeSureIsValid(ref inputFromUserToCheckValidation);
            }

             makeSureIsValid(ref inputFromUserToCheckValidation);
            if (userPressToQuit(inputFromUserToCheckValidation) == true)
            {
                o_DataFromUser = k_IrrelevantVariable;
                if (io_CurrentPlayer.Number == ePlayerNumber.One)
                {
                    s_Game.StatusGame = eCurrentStatusOfTheGame.PlayerOneChoseToQuitTheGame;
                }
                else
                {
                    s_Game.StatusGame = eCurrentStatusOfTheGame.PlayerTwoChoseToQuitTheGame;
                }
            }
            else
            {
                makeSureIsValid(ref inputFromUserToCheckValidation);
                o_DataFromUser = int.Parse(inputFromUserToCheckValidation);
                while (s_Game.Board.BoardSize < o_DataFromUser || o_DataFromUser <= 0)
                {
                    Console.WriteLine($"please enter new input betwen 1 to {s_Game.Board.BoardSize}");
                    getDataFromUser(out o_DataFromUser, ref io_CurrentPlayer);
                }
            }
        }

        private static bool userChooseEnterInsteadNumber(string i_UserInput)
        {
            bool userChooseEnter = false;

            if(i_UserInput.Length == 0)
            {
                userChooseEnter = true;
            }

            return userChooseEnter;
        }

        private static byte getBoardSize()
        {
            byte boardSize;
            bool isByteInput = false;
            string msg;

            msg = string.Format(@"Please enter the size of 'X MIX DRIX' game board. The size must be between 3 to 9 only.");
            Console.WriteLine(msg);
            isByteInput = byte.TryParse(Console.ReadLine(), out boardSize);
            while (!isByteInput || Game.IsTheBoardSizeInRange(boardSize) != true)
            {
                if (!isByteInput)
                {
                    Console.WriteLine("Invaild input, You must enter a number. Let's try again!");
                }
                else
                {
                    Console.WriteLine("The input exceeds the possible size range of the game board, let's try again!");
                }

                isByteInput = byte.TryParse(Console.ReadLine(), out boardSize);
            }

            return boardSize;
        }

        private static bool checkIfTheGameIsAgainstTheComputer()
        {
            Console.Write("Would you like to play against the computer? ");

            return userAnswerYesToTheQuestion();
        }

        private static bool userAnswerYesToTheQuestion()
        {
            bool userDecidedYes = false;
            string userDecision;

            Console.WriteLine(@"choose 'y' (for yes) or 'n' (for no).");
            userDecision = Console.ReadLine();
            while (userDecision != "y" && userDecision != "n" && userDecision != "Y" && userDecision != "N")
            {
                Console.WriteLine("It's a yes or no question only, let's try again.");
                userDecision = Console.ReadLine();
            }

            userDecidedYes = (userDecision == "y" || userDecision == "Y") ? true : false;

            return userDecidedYes;
        }

        private static void checkIfUserChooseToPlayOntherRound()
        {
            Console.WriteLine("Do You Want To Play Again?");
            if(userAnswerYesToTheQuestion())
            {
                s_Game.StatusGame = eCurrentStatusOfTheGame.PlayingGame;
                s_Game.InitializationTheBoardForNewRound(s_Game.Board.BoardSize);
                printBoard();
                playGameLogicMoves();
            }
            else
            {
                Console.WriteLine("bye bye! hope you have good time!");
            }    
        }

        private static void buildGame()
        {
            byte boardSize;
            string firstPlayerName, secondPlayerName;
            bool isComputerPlaying;

            boardSize = getBoardSize();
            firstPlayerName = getPlayerName(k_FirstPlayerNumber);
            isComputerPlaying = checkIfTheGameIsAgainstTheComputer();
            if (isComputerPlaying)
            {
                s_Game = new Game(isComputerPlaying, firstPlayerName, "Computer", boardSize);
            }
            else
            {
                secondPlayerName = getPlayerName(k_SecondPlayerNumber);
                s_Game = new Game(isComputerPlaying, firstPlayerName, secondPlayerName, boardSize);
            }
        }

        private static void makeSureIsValid(ref string io_InputFromUser)
        {
            while(checkIfInputIsValid(ref io_InputFromUser) == false)
            {
                Console.WriteLine("wrong input, please enter good input");
                io_InputFromUser = Console.ReadLine();
            }
        }

        private static bool checkIfInputIsValid(ref string i_InputFromUser)
        {
            bool inputIsValid = false;

            if (checkIfInputIsNumber(i_InputFromUser) == true || userPressToQuit(i_InputFromUser) || false || userChooseEnterInsteadNumber(i_InputFromUser) == true)
            {
                inputIsValid = true;
            }

            return inputIsValid;
        }

        private static bool userPressToQuit(string i_InputFromUser)
        {
            bool userPressToQuit = false;

            if(char.ToString(k_QuitGameSign) == i_InputFromUser)
            {
                userPressToQuit = true;
            }

            return userPressToQuit;
        }

        private static bool checkIfInputIsNumber(string i_InputFromUser)
        {
            bool inputIsValid = true;

            for (int i = 0; i < i_InputFromUser.Length; i++)
            {
                if (char.IsDigit(i_InputFromUser[i]) == false)
                {
                    inputIsValid = false;
                }
            }

            return inputIsValid;
        }

        private static void printBoard()
        {
            StringBuilder boardGame = new StringBuilder();

            Ex02.ConsoleUtils.Screen.Clear();
            printColumnsNumbersLine(ref boardGame);

            for (byte row = 0; row < s_Game.Board.BoardSize; row++)
            {
                printCurrentRowAtBoard(ref boardGame, row);
            }

            Console.WriteLine(boardGame);
        }

        private static void printCurrentRowAtBoard(ref StringBuilder io_BoardGame, byte i_RowNumber)
        {
            io_BoardGame.Append(i_RowNumber + 1);
            printDataOfCurrentRow(ref io_BoardGame, i_RowNumber);
            printSeparationOfLines(ref io_BoardGame);
        }

        private static void printDataOfCurrentRow(ref StringBuilder io_BoardGame, byte i_Row)
        {
            for (int col = 0; col < s_Game.Board.BoardSize; col++)
            {
                io_BoardGame.Append("| " + s_Game.Board[i_Row, col].Sign + " ");

                if (col == s_Game.Board.BoardSize - 1)
                {
                    io_BoardGame.Append("|");
                }
            }

            io_BoardGame.Append(Environment.NewLine);
        }

        private static void printColumnsNumbersLine(ref StringBuilder io_BoardGame)
        {
            for (int col = 0; col < s_Game.Board.BoardSize; col++)
            {
                io_BoardGame.Append("   " + (col + 1));
            }

            io_BoardGame.Append(Environment.NewLine);
        }

        private static void printSeparationOfLines(ref StringBuilder io_BoardGame)
        {
            for (int sizeBoardRow = 0; sizeBoardRow < s_Game.Board.BoardSize; sizeBoardRow++)
            {
                if (sizeBoardRow == 0)
                {
                    io_BoardGame.Append("  ");
                }

                io_BoardGame.Append("====");
            }

            io_BoardGame.Append(Environment.NewLine);
        }

        private static string getPlayerName(byte i_PlayerNumber)
        {
            string playerName;
            string msg;

            msg = string.Format("Please enter the name of player number {0}:", i_PlayerNumber);
            Console.WriteLine(msg);
            playerName = Console.ReadLine();
            while (playerName.Length == 0)
            {
                Console.WriteLine("You are must to choose player name.");
                playerName = Console.ReadLine();
            }

            return playerName;
        }

       private static void getCoordinateFromCurrentPlayer(ref Player io_CurentPlayer, out int row, out int col)
        {
            if(io_CurentPlayer.Name == "Computer")
            {
                s_Game.GetCoordinateFromComputer(out row, out col);
            }
            else
            {
                getCoordinateFromUser(out row, out col, ref io_CurentPlayer);
            }
        }
    }
}

