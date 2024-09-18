using System.Data;
using System.Text;

namespace Chessboard;
// Mowitz Almstedt -NET24 //

/*=========================================================================
 * This is a simple program that displays a chessboard with pre-made values
 * that is customizable via an option menu. The user can both change the
 * size by x*y configuration, but also change the character displaying
 * both black and white squares. It's always possible for the user to
 * return to the main menu as the program runs until quit is chosen.
 *
 * I tried implementing a way to replace a square with a chesspiece
 * chosen by the user stored in a dictionary, by using tags
 * attached to the rows and columns, but to no avail.
 =========================================================================*/

public class Program
{
    //List to define (white) chess pieces and their Unicode equivalent
    private static readonly Dictionary<string, string> ChessPiece = new Dictionary<string, string>
    {
        { "KING", "\u2654" }, //Unicode for king
        { "QUEEN", "\u2655" }, //Unicode for queen
        { "ROOK", "\u2656" }, //Unicode for rook
        { "BISHOP", "\u2657" }, //Unicode for bishop
        { "KNIGHT", "\u2658" }, //Unicode for knight
        { "PAWN", "\u2659" }, //Unicode for pawn
    };
        
    public static void Main(string[] args)
    {
        //Allows the console to read and print UTF-8 Characters
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int boardSize = 8; //Default board size (8x8)
        string whiteSquare = "\u25a1"; //UTF-8 Code for white square
        string blackSquare = "\u25a0"; //UTF-8 Code for black square
        bool isRunning = true;
        string[,] board = new string[boardSize, boardSize]; //Array for the board
        
        while (isRunning) //Control the main menu loop
        {
            int menuChoice = ShowMainMenu(); //Get user input from main menu
            
            switch (menuChoice)
            {
                case 1: //Play
                    bool shouldRepeat = true; //Control the options menu loop
                    
                    while (shouldRepeat)
                    {
                        Console.Clear(); //Clear the console
                        PrintBoard(boardSize, whiteSquare, blackSquare, board); //Display current board
                        int optionChoice = ShowOptionsMenu(); //Get user input from options menu

                        switch (optionChoice)
                        {
                            case 1: //Customize board size
                                boardSize = GetValidBoardSize();
                                board = new string[boardSize, boardSize]; //Reset the board with the new size
                                break;
                            
                            case 2: //Customize squares
                                whiteSquare = GetCustomCharacter("White");
                                blackSquare = GetCustomCharacter("Black");
                                break;
                            
                            case 3: //Place a piece
                                //PlacePieceOnBoard(board, boardSize);
                                break;
                            
                            case 4: //Return
                                Console.Clear();
                                shouldRepeat = false; //Exit the options loop
                                break;
                        }
                    }
                    break;
                
                case 2: //Quit
                    Console.WriteLine("Quitting!");
                    isRunning = false; //Exit the main loop and quit the program
                    break;
            }
        }
    }

    //Method to show main menu
    public static int ShowMainMenu()
    {
        Console.WriteLine("======================");
        Console.WriteLine("Welcome to Chessboard!");
        Console.WriteLine("======================");
        Console.WriteLine("[1] Play");
        Console.WriteLine("[2] Quit");
        Console.Write("[ ]");
        Console.SetCursorPosition(1 , Console.CursorTop);
        
        return GetValidInput(1, 2);
    }

    //Method to show options menu
    public static int ShowOptionsMenu() 
    {
        
        Console.WriteLine("Customize Board!");
        Console.WriteLine("[1] Change Board Size");
        Console.WriteLine("[2] Custom Squares");
        Console.WriteLine("[3] Place a piece");
        Console.WriteLine("[4] Return to Main Menu");
        Console.Write("[ ]");
        Console.SetCursorPosition(1 , Console.CursorTop);
        
        return GetValidInput(1, 4);
    }

    //Method to print the current state of the board
    public static void PrintBoard(int boardSize, string whiteSquare, string blackSquare, string[,] board)
    {
        //Print column numbers
        Console.Write("   ");
        for (int colTag = 1; colTag <= boardSize; colTag++)
        {
            Console.Write($" {colTag} ");
        }
        Console.WriteLine();
        
        //Print each row on the board
        for (int i = 0; i < boardSize; i++)
        {
            //Add tags to each row (A, B, C etc.)
            char rowTag = (char)('A' + (boardSize - i - 1)); 
            Console.Write($" {rowTag} ");
            
            //Print each column
            for (int j = 0; j < boardSize; j++)
            {
                //Determine color of each square
                string square = (i + j) % 2 == 0 ? whiteSquare : blackSquare;
                Console.Write($" {square} ");

                //Print a chess piece if present
                if (board[i, j] != null)
                {
                    Console.Write($" {board[i, j]} ");
                }
                else
                {
                    Console.Write("");
                }
            }
            Console.WriteLine();
        }
    }

    //Method to place a piece on the board
    public static void PlacePieceOnBoard(string[,] board, int boardSize)
    {
        Console.Write("Enter a position (ex. B4): ");
        string position = Console.ReadLine().ToUpper(); //Get position from user input
        
        Console.Write("Enter a piece type (ex. Rook): "); //Get piece type from user input
        string pieceType = Console.ReadLine().ToUpper();

        //Check for piece and position validation
        if (ChessPiece.TryGetValue(pieceType, out string pieceChar))
        {
            try
            {
                //Convert position to index
                (int row, int col) = ParsePosition(position, boardSize);
                board[row, col] = pieceChar; //Place the piece on the board
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error placing piece: {exception.Message}"); //Handle errors
            }
        }
        else
        {
            Console.WriteLine("Invalid piece type"); //Handle invalid piece types (ex. typos)
        }
    }
    
    //Method to convert board tags (ex. B4) to board index 
    public static (int row, int col) ParsePosition(string position, int boardSize)
    {
        if (position.Length < 2)
            throw new ArgumentException("Invalid position");
        
        //Handles column conversion
        char colChar = position[0];
        int col = colChar - 'A'; //Convert column letter to number ex. A = 0, B = 1 etc.
        
        //Handles row conversion
        if (!int.TryParse(position.Substring(1), out int row))
            throw new ArgumentException("Invalid position");
        
        row = boardSize - row; //Convert index to be zero-based

        //Check if input is within board boundaries
        if (row < 0 || row >= boardSize || col < 0 || col >= boardSize)
            throw new ArgumentException("Position out of bounds");
        
        return (row, col);
    }

    //Method to check valid integer input within a specified range
    public static int GetValidInput(int min, int max)
    {
        int input;
        do
        {
            if (int.TryParse(Console.ReadLine(), out input) && input >= min && input <= max)
            {
                break;
            }
            Console.Write($"Invalid input\nEnter a number between {min} and {max}: ");
        } while (true);
        
        return input;
    }
    
    //Method to validate new board size input
    public static int GetValidBoardSize()
    { 
        Console.Write("Enter a number between 1 and 9: ");
        return GetValidInput(1, 9);
    }

    //Method to update square values from user input
    public static string GetCustomCharacter(string squareType)
    {
        Console.Write($"Enter a custom character for {squareType}: ");
        return Console.ReadLine();
    }

}