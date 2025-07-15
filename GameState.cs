using System.Collections.Generic;

namespace Chess
{
    public static class GameState
    {
        // Player settings
        public static string Player1Name = "Player 1";
        public static string Player2Name = "Player 2";
        public static int TimeControl = 120; // seconds
        public static bool TimeControlEnabled = true;
        
        // Game timing
        public static int WhiteTimeLeft;
        public static int BlackTimeLeft;
        
        // Display settings
        public static bool ShowBoardSquareNumbers = false;
        
        // Game state
        public static List<Move> CurrentLegalMoves = new List<Move>();
        public static string LastMove = "";
        
        // Initialize timer values based on time control
        public static void InitializeTimers()
        {
            if (TimeControlEnabled && TimeControl > 0)
            {
                WhiteTimeLeft = TimeControl;
                BlackTimeLeft = TimeControl;
            }
            else
            {
                WhiteTimeLeft = 0;
                BlackTimeLeft = 0;
            }
        }
        
        public static void SetPlayer1Name(string name)
        {
            Player1Name = name;
        }

        public static void SetPlayer2Name(string name)
        {
            Player2Name = name;
        }

        public static void SetTimeControl(int seconds)
        {
            TimeControl = seconds;
        }

        public static void ToggleTimeControl(bool enabled)
        {
            TimeControlEnabled = enabled;
        }
        
        // Generate legal moves for current position
        public static void SetLegalMoves()
        {
            CurrentLegalMoves = RuleBook.GenerateLegalMoves();
            
            // Check for game end conditions
            if (CurrentLegalMoves.Count == 0 && RuleBook.KingInCheck)
            {
                // Checkmate - opposing side wins
                GameControl.EndGame(GameControl.OpposingSide());
            }
            else if (CurrentLegalMoves.Count == 0 && !RuleBook.KingInCheck)
            {
                // Stalemate
                GameControl.EndGame(-1);
            }
        }
        
        // Update move list display
        public static void UpdateMoveDisplay(string moveDescription)
        {
            // For now, just store the last move description
            // In a full implementation, this could maintain a list of moves
            LastMove = moveDescription;
        }
    }
} 