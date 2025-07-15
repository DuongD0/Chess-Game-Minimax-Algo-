using System;
using System.IO;
using System.Text;

namespace Chess
{
    public static class CsvMoveLogger
    {
        private static string? _filePath;
        private static StringBuilder _csvBuilder = new StringBuilder();

        public static void Initialize(string fileName = "chess_moves.csv")
        {
            _filePath = Path.Combine(AppContext.BaseDirectory, fileName);
            Console.WriteLine($"[CSV Logger] Initialized. Path: {_filePath}");
            _csvBuilder.Clear();
            _csvBuilder.AppendLine("MoveNumber,Piece,From,To,Notation");
            Flush(); 
        }

        public static void LogMove(int moveNumber, Move move, string notation)
        {
            string pieceName = Piece.PieceToFullName(move.Piece);
            string fromSquare = $"{ (char)('a' + move.MoveFrom % 8) }{ 8 - move.MoveFrom / 8 }";
            string toSquare = $"{ (char)('a' + move.MoveTo % 8) }{ 8 - move.MoveTo / 8 }";

            _csvBuilder.AppendFormat("{0},{1},{2},{3},{4}\n",
                moveNumber,
                pieceName,
                fromSquare,
                toSquare,
                notation);
        }

        public static void Flush()
        {
            if (_csvBuilder.Length > 0 && _filePath != null)
            {
                Console.WriteLine("[CSV Logger] Flushing moves to CSV...");
                try
                {
                    File.AppendAllText(_filePath, _csvBuilder.ToString());
                    Console.WriteLine("[CSV Logger] Flush successful.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[CSV Logger] Error writing to CSV: {ex.Message}");
                }
                _csvBuilder.Clear();
            }
        }
    }
} 