using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace Chess
{
    public class GameRenderer
    {
        // Board constants
        private const int TILE_SIZE = 75;
        private const int BOARD_START_X = 40;
        private const int BOARD_START_Y = 40;
        private const int BOARD_BORDER_WIDTH = 3;
        
        // Enhanced color palette
        private static readonly Color LIGHT_SQUARE = SplashKit.RGBColor(240, 217, 181);
        private static readonly Color DARK_SQUARE = SplashKit.RGBColor(181, 136, 99);
        private static readonly Color BOARD_BORDER = SplashKit.RGBColor(101, 67, 33);
        private static readonly Color BACKGROUND = SplashKit.RGBColor(45, 52, 64);
        
        // Selection and move colors
        private static readonly Color SELECTED_SQUARE = SplashKit.RGBColor(255, 255, 102);
        private static readonly Color LEGAL_MOVE = SplashKit.RGBColor(124, 252, 0);
        private static readonly Color LAST_MOVE_FROM = SplashKit.RGBColor(255, 206, 84);
        private static readonly Color LAST_MOVE_TO = SplashKit.RGBColor(255, 206, 84);
        private static readonly Color CHECK_HIGHLIGHT = SplashKit.RGBColor(255, 69, 69);
        
        // UI colors
        private static readonly Color PANEL_BACKGROUND = SplashKit.RGBColor(67, 76, 94);
        private static readonly Color PANEL_BORDER = SplashKit.RGBColor(129, 161, 193);
        private static readonly Color BUTTON_NORMAL = SplashKit.RGBColor(88, 101, 242);
        private static readonly Color BUTTON_HOVER = SplashKit.RGBColor(118, 131, 252);
        private static readonly Color TEXT_PRIMARY = SplashKit.ColorWhite();
        private static readonly Color TEXT_SECONDARY = SplashKit.RGBColor(216, 222, 233);
        private static readonly Color ACCENT_COLOR = SplashKit.RGBColor(235, 203, 139);
        
        // Piece images
        private Dictionary<string, Bitmap>? pieceImages;
        
        // Button rectangles for click detection
        public Rectangle MenuButton1 { get; private set; }
        public Rectangle MenuButton2 { get; private set; }
        public Rectangle MenuButton3 { get; private set; }
        public Rectangle MenuButton4 { get; private set; }
        public Rectangle ResignButton { get; private set; }
        public Rectangle MenuFromGameButton { get; private set; }
        public Rectangle BackButton { get; private set; }
        public Rectangle Player1NameBox { get; private set; }
        public Rectangle Player2NameBox { get; private set; }
        public Rectangle TimeUpButton { get; private set; }
        public Rectangle TimeDownButton { get; private set; }
        public Rectangle TimeToggleButton { get; private set; }
        public Rectangle FenInputBox { get; private set; }
        
        private Font _gameFont;
        
        public GameRenderer()
        {
            LoadPieceImages();
            _gameFont = SplashKit.LoadFont("Default", "arial.ttf");
            MenuButton1 = new Rectangle();
            MenuButton2 = new Rectangle();
            MenuButton3 = new Rectangle();
            MenuButton4 = new Rectangle();
            ResignButton = new Rectangle();
            MenuFromGameButton = new Rectangle();
            BackButton = new Rectangle();
            Player1NameBox = new Rectangle();
            Player2NameBox = new Rectangle();
            TimeUpButton = new Rectangle();
            TimeDownButton = new Rectangle();
            TimeToggleButton = new Rectangle();
            FenInputBox = new Rectangle();
        }
        
        private void LoadPieceImages()
        {
            pieceImages = new Dictionary<string, Bitmap>();
            
            try
            {
                // Load white pieces
                pieceImages["WKing"] = SplashKit.LoadBitmap("WKing", "Piece Images/White/WKing.png");
                pieceImages["WQueen"] = SplashKit.LoadBitmap("WQueen", "Piece Images/White/WQueen.png");
                pieceImages["WRook"] = SplashKit.LoadBitmap("WRook", "Piece Images/White/WRook.png");
                pieceImages["WBishop"] = SplashKit.LoadBitmap("WBishop", "Piece Images/White/WBishop.png");
                pieceImages["WKnight"] = SplashKit.LoadBitmap("WKnight", "Piece Images/White/WKnight.png");
                pieceImages["WPawn"] = SplashKit.LoadBitmap("WPawn", "Piece Images/White/WPawn.png");
                
                // Load black pieces
                pieceImages["BKing"] = SplashKit.LoadBitmap("BKing", "Piece Images/Black/BKing.png");
                pieceImages["BQueen"] = SplashKit.LoadBitmap("BQueen", "Piece Images/Black/BQueen.png");
                pieceImages["BRook"] = SplashKit.LoadBitmap("BRook", "Piece Images/Black/BRook.png");
                pieceImages["BBishop"] = SplashKit.LoadBitmap("BBishop", "Piece Images/Black/BBishop.png");
                pieceImages["BKnight"] = SplashKit.LoadBitmap("BKnight", "Piece Images/Black/BKnight.png");
                pieceImages["BPawn"] = SplashKit.LoadBitmap("BPawn", "Piece Images/Black/BPawn.png");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load piece images: {ex.Message}");
            }
        }
        
        public void DrawMenu(Window window)
        {
            DrawCleanBackground(window);
            
            double centerX = window.Width / 2;
            DrawTextWithShadow("CHESS MASTER", 42, centerX - 120, 80, ACCENT_COLOR, SplashKit.ColorBlack(), window);
            
            double startY = 180;
            double buttonSpacing = 70;
            double buttonWidth = 280;
            double buttonHeight = 55;
            
            MenuButton1 = new Rectangle() { X = centerX - buttonWidth/2, Y = startY, Width = buttonWidth, Height = buttonHeight };
            MenuButton2 = new Rectangle() { X = centerX - buttonWidth/2, Y = startY + buttonSpacing, Width = buttonWidth, Height = buttonHeight };
            MenuButton3 = new Rectangle() { X = centerX - buttonWidth/2, Y = startY + buttonSpacing * 2, Width = buttonWidth, Height = buttonHeight };
            MenuButton4 = new Rectangle() { X = centerX - buttonWidth/2, Y = startY + buttonSpacing * 3, Width = buttonWidth, Height = buttonHeight };
            
            DrawMenuButton("1. Single Player", MenuButton1, BUTTON_NORMAL, window);
            DrawMenuButton("2. Two Player", MenuButton2, BUTTON_NORMAL, window);
            DrawMenuButton("3. Options", MenuButton3, BUTTON_NORMAL, window);
            DrawMenuButton("4. Exit", MenuButton4, SplashKit.RGBColor(191, 97, 106), window);
            
            DrawText("Click on buttons or use number keys", 16, centerX - 110, startY + buttonSpacing * 4 + 40, TEXT_SECONDARY, window);
        }
        
        public void DrawOptions(Window window)
        {
            DrawCleanBackground(window);
            
            double centerX = window.Width / 2;
            DrawTextWithShadow("GAME OPTIONS", 36, centerX - 120, 50, ACCENT_COLOR, SplashKit.ColorBlack(), window);
            
            double panelX = 100;
            double panelY = 110;
            double panelWidth = window.Width - 200;
            double panelHeight = 520;
            
            DrawPanel(panelX, panelY, panelWidth, panelHeight, window);
            
            double contentX = panelX + 40;
            double currentY = panelY + 30;
            
            double sectionPadding = 65;
            double itemPadding = 50;
            double rowHeight = 40;

            // --- Player Settings ---
            DrawSectionHeader("Player Settings", contentX, currentY, window);
            currentY += 50;
            
            if (pieceImages != null && _gameFont != null)
            {
                // Player 1 Row
                DrawText("Player 1:", 16, contentX + 50, currentY + (rowHeight - SplashKit.TextHeight("Player 1:", _gameFont, 16)) / 2, TEXT_PRIMARY, window);
                window.DrawBitmap(pieceImages["WKing"], contentX, currentY + (rowHeight - pieceImages["WKing"].Height * 0.6) / 2 - 20, SplashKit.OptionScaleBmp(0.6, 0.6));
                Player1NameBox = new Rectangle() { X = contentX + 150, Y = currentY + (rowHeight - 35) / 2, Width = panelWidth - 240, Height = 35 };
                DrawTextInputBox(GameState.Player1Name, Player1NameBox, TextInputManager.ActiveField.Player1, window);
                currentY += itemPadding;

                // Player 2 Row
                DrawText("Player 2:", 16, contentX + 50, currentY + (rowHeight - SplashKit.TextHeight("Player 2:", _gameFont, 16)) / 2, TEXT_PRIMARY, window);
                window.DrawBitmap(pieceImages["BKing"], contentX, currentY + (rowHeight - pieceImages["BKing"].Height * 0.6) / 2 - 20, SplashKit.OptionScaleBmp(0.6, 0.6));
                Player2NameBox = new Rectangle() { X = contentX + 150, Y = currentY + (rowHeight - 35) / 2, Width = panelWidth - 240, Height = 35 };
                DrawTextInputBox(GameState.Player2Name, Player2NameBox, TextInputManager.ActiveField.Player2, window);
            }
            
            // --- Time Control ---
            currentY += sectionPadding;
            DrawSectionHeader("Time Control", contentX, currentY, window);
            currentY += 50;

            TimeToggleButton = new Rectangle() { X = contentX, Y = currentY + (rowHeight - 35) / 2, Width = 230, Height = 35 };
            DrawMenuButton(GameState.TimeControlEnabled ? "Time Control: Enabled" : "Time Control: Disabled", TimeToggleButton, BUTTON_NORMAL, window);

            if (GameState.TimeControlEnabled && _gameFont != null)
            {
                string timeText = $"{GameState.TimeControl / 60}:{GameState.TimeControl % 60:D2}";
                TimeDownButton = new Rectangle() { X = contentX + 350, Y = currentY + (rowHeight - 35) / 2, Width = 35, Height = 35 };
                DrawMenuButton("-", TimeDownButton, BUTTON_NORMAL, window);

                DrawText(timeText, 28, contentX + 405, currentY + (rowHeight - SplashKit.TextHeight(timeText, _gameFont, 28))/2, ACCENT_COLOR, window);
                
                TimeUpButton = new Rectangle() { X = contentX + 480, Y = currentY + (rowHeight - 35) / 2, Width = 35, Height = 35 };
                DrawMenuButton("+", TimeUpButton, BUTTON_NORMAL, window);
            }

            // --- FEN Settings ---
            currentY += sectionPadding;
            DrawSectionHeader("Starting Position (FEN)", contentX, currentY, window);
            currentY += 50;
            
            FenInputBox = new Rectangle() { X = contentX, Y = currentY, Width = panelWidth - 80, Height = 60 };
            DrawTextInputBox(FenStringUtility.InputedPostion, FenInputBox, TextInputManager.ActiveField.FEN, window);

            // --- Back Button ---
            BackButton = new Rectangle() { X = centerX - 100, Y = panelY + panelHeight - 60, Width = 200, Height = 40 };
            DrawMenuButton("Back to Menu", BackButton, BUTTON_NORMAL, window);
        }
        
        private void DrawSectionHeader(string title, double x, double y, Window window)
        {
            DrawText(title, 20, x, y, ACCENT_COLOR, window);
            window.FillRectangle(PANEL_BORDER, x, y + 28, 300, 2);
        }
        
        public void DrawGame(Window window)
        {
            DrawCleanBackground(window);
            DrawBoardWithBorder(window);
            DrawBoardNotation(window);
            DrawPieces(window);
            DrawGameUI(window);
            
            if (GameControl.gameEnded)
            {
                DrawGameOverOverlay(window);
            }
        }
        
        private void DrawCleanBackground(Window window)
        {
            window.Clear(BACKGROUND);
            
            for (int x = 0; x < window.Width; x += 120)
            {
                for (int y = 0; y < window.Height; y += 120)
                {
                    Color patternColor = SplashKit.RGBColor(
                        Math.Min(255, BACKGROUND.R + 8),
                        Math.Min(255, BACKGROUND.G + 8),
                        Math.Min(255, BACKGROUND.B + 8)
                    );
                    window.FillCircle(patternColor, x + 60, y + 60, 2);
                }
            }
        }
        
        private void DrawBoardWithBorder(Window window)
        {
            double shadowOffset = 6;
            window.FillRectangle(SplashKit.RGBColor(15, 15, 15), 
                BOARD_START_X + shadowOffset, BOARD_START_Y + shadowOffset, 
                8 * TILE_SIZE + BOARD_BORDER_WIDTH * 2, 8 * TILE_SIZE + BOARD_BORDER_WIDTH * 2);
            
            window.FillRectangle(BOARD_BORDER, 
                BOARD_START_X - BOARD_BORDER_WIDTH, BOARD_START_Y - BOARD_BORDER_WIDTH,
                8 * TILE_SIZE + BOARD_BORDER_WIDTH * 2, 8 * TILE_SIZE + BOARD_BORDER_WIDTH * 2);
            
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    int squareIndex = row * 8 + col;
                    double x = BOARD_START_X + col * TILE_SIZE;
                    double y = BOARD_START_Y + row * TILE_SIZE;
                    
                    Color squareColor = GetSquareColor(squareIndex, row, col);
                    
                    DrawSquareWithGradient(x, y, TILE_SIZE, squareColor, window);
                }
            }
        }
        
        private Color GetSquareColor(int squareIndex, int row, int col)
        {
            if (GameControl.Board?[squareIndex] != null)
            {
                Color currentColor = GameControl.Board[squareIndex].BackgroundColor;
                
                if (!ColorEquals(currentColor, LIGHT_SQUARE) && !ColorEquals(currentColor, DARK_SQUARE))
                {
                    return currentColor;
                }
            }
            
            return ((row + col) % 2 == 0) ? LIGHT_SQUARE : DARK_SQUARE;
        }
        
        private bool ColorEquals(Color c1, Color c2)
        {
            return c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;
        }
        
        private void DrawSquareWithGradient(double x, double y, double size, Color baseColor, Window window)
        {
            window.FillRectangle(baseColor, x, y, size, size);
            
            Color lighterColor = SplashKit.RGBColor(
                Math.Min(255, baseColor.R + 12),
                Math.Min(255, baseColor.G + 12),
                Math.Min(255, baseColor.B + 12)
            );
            
            window.DrawLine(lighterColor, x, y, x + size - 1, y);
            window.DrawLine(lighterColor, x, y, x, y + size - 1);
        }
        
        private void DrawBoardNotation(Window window)
        {
            for (int col = 0; col < 8; col++)
            {
                char fileLetter = (char)('a' + col);
                double x = BOARD_START_X + col * TILE_SIZE + TILE_SIZE / 2 - 6;
                double y = BOARD_START_Y + 8 * TILE_SIZE + 12;
                DrawTextWithShadow(fileLetter.ToString(), 16, x, y, TEXT_PRIMARY, SplashKit.ColorBlack(), window);
            }
            
            for (int row = 0; row < 8; row++)
            {
                int rankNumber = 8 - row;
                double x = BOARD_START_X - 25;
                double y = BOARD_START_Y + row * TILE_SIZE + TILE_SIZE / 2 - 8;
                DrawTextWithShadow(rankNumber.ToString(), 16, x, y, TEXT_PRIMARY, SplashKit.ColorBlack(), window);
            }
        }
        
        private void DrawPieces(Window window)
        {
            if (pieceImages == null) return;
            
            for (int i = 0; i < 64; i++)
            {
                if (GameControl.Board[i] != null && GameControl.Board[i].PieceOnSquare != 0)
                {
                    string imageName = GetPieceImageName(GameControl.Board[i].PieceOnSquare);
                    if (pieceImages.ContainsKey(imageName))
                    {
                        int row = i / 8;
                        int col = i % 8;
                        double x = BOARD_START_X + col * TILE_SIZE + 4;
                        double y = BOARD_START_Y + row * TILE_SIZE + 4;
                        
                        double pieceSize = TILE_SIZE - 8;
                        window.DrawBitmap(pieceImages[imageName], x, y, 
                            SplashKit.OptionScaleBmp(pieceSize / 64, pieceSize / 64));
                    }
                }
            }
        }
        
        private void DrawGameUI(Window window)
        {
            double panelX = BOARD_START_X + 8 * TILE_SIZE + 20;
            double panelY = BOARD_START_Y;
            double panelWidth = 280;
            double panelHeight = 8 * TILE_SIZE;
            
            DrawPanel(panelX, panelY, panelWidth, panelHeight, window);
            
            double contentX = panelX + 20;
            double contentY = panelY + 20;
            
            DrawTextWithShadow("GAME STATUS", 18, contentX, contentY, ACCENT_COLOR, SplashKit.ColorBlack(), window);
            contentY += 40;
            
            string currentPlayer = GameControl.sideToMove == 8 ? "White" : "Black";
            Color playerColor = GameControl.sideToMove == 8 ? SplashKit.ColorWhite() : SplashKit.ColorBlack();
            
            DrawText("Current Player:", 14, contentX, contentY, TEXT_SECONDARY, window);
            float labelWidth = SplashKit.TextWidth("Current Player:", _gameFont, 14);
            double indicatorX = contentX + labelWidth + 20;
            window.FillCircle(playerColor, indicatorX, contentY + 8, 8);
            window.DrawCircle(TEXT_PRIMARY, indicatorX, contentY + 8, 8);
            DrawText(currentPlayer, 14, indicatorX + 20, contentY, TEXT_PRIMARY, window);
            contentY += 30;
            
            if (GameState.TimeControl > 0)
            {
                contentY += 15;
                DrawText("TIME REMAINING", 14, contentX, contentY, ACCENT_COLOR, window);
                contentY += 25;
                
                DrawTimerDisplay("White:", GameState.WhiteTimeLeft, GameState.TimeControl, contentX, contentY, window);
                contentY += 35;
                DrawTimerDisplay("Black:", GameState.BlackTimeLeft, GameState.TimeControl, contentX, contentY, window);
                contentY += 40;
            }
            
            DrawText($"Moves: {GameControl.Moves}", 14, contentX, contentY, TEXT_PRIMARY, window);
            contentY += 40;
            
            ResignButton = new Rectangle() { X = contentX, Y = contentY, Width = 110, Height = 35 };
            MenuFromGameButton = new Rectangle() { X = contentX + 120, Y = contentY, Width = 80, Height = 35 };
            
            DrawGameButton("Resign", ResignButton, SplashKit.RGBColor(191, 97, 106), window);
            DrawGameButton("Menu", MenuFromGameButton, BUTTON_NORMAL, window);
            contentY += 55;
            
            DrawMoveHistory(contentX, contentY, panelWidth - 40, window);
        }
        
        private void DrawTimerDisplay(string label, int timeLeft, int totalTime, double x, double y, Window window)
        {
            int minutes = timeLeft / 60;
            int seconds = timeLeft % 60;
            
            DrawText(label, 12, x, y, TEXT_SECONDARY, window);
            DrawText($"{minutes:D2}:{seconds:D2}", 14, x + 50, y, TEXT_PRIMARY, window);
            
            double barWidth = 150;
            double barHeight = 8;
            double progress = Math.Max(0, (double)timeLeft / totalTime);
            
            window.FillRectangle(SplashKit.RGBColor(40, 40, 40), x, y + 20, barWidth, barHeight);
            
            Color progressColor = progress > 0.3 ? SplashKit.RGBColor(163, 190, 140) : 
                                 progress > 0.1 ? SplashKit.RGBColor(235, 203, 139) : SplashKit.RGBColor(191, 97, 106);
            window.FillRectangle(progressColor, x, y + 20, barWidth * progress, barHeight);
            
            window.DrawRectangle(TEXT_SECONDARY, x, y + 20, barWidth, barHeight);
        }
        
        private void DrawMoveHistory(double x, double y, double width, Window window)
        {
            DrawText("MOVE HISTORY", 14, x, y, ACCENT_COLOR, window);
            y += 25;
            
            double listHeight = 150;
            window.FillRectangle(SplashKit.RGBColor(30, 30, 30), x, y, width, listHeight);
            window.DrawRectangle(PANEL_BORDER, x, y, width, listHeight);
            
            DrawText("White", 12, x + 10, y + 5, TEXT_SECONDARY, window);
            DrawText("Black", 12, x + width/2 + 10, y + 5, TEXT_SECONDARY, window);
            
            int totalMoves = MoveStack.moveStack.Count;
            int displayLimit = 12; // Max moves to display
            int startMoveIndex = Math.Max(0, totalMoves - displayLimit);

            double moveY = y + 25;
            
            for (int i = startMoveIndex; i < totalMoves && moveY < y + listHeight - 15; i++)
            {
                Move move = MoveStack.moveStack[i];
                string moveText = MoveStack.MoveToDescriptiveNotation(move);
                bool isWhiteMove = Piece.IsColour(move.Piece, Piece.White);
                
                int moveNumber = (i / 2) + 1;
                
                if (isWhiteMove)
                {
                    DrawText($"{moveNumber}.", 10, x + 5, moveY, TEXT_SECONDARY, window);
                    DrawText(moveText, 10, x + 25, moveY, TEXT_PRIMARY, window);
                }
                else
                {
                    DrawText(moveText, 10, x + width/2 + 10, moveY, TEXT_PRIMARY, window);
                    moveY += 15;
                }
            }
        }
        
        private void DrawGameOverOverlay(Window window)
        {
            window.FillRectangle(SplashKit.RGBAColor(0, 0, 0, 150), 0, 0, window.Width, window.Height);
            
            double panelWidth = 400;
            double panelHeight = 200;
            double panelX = (window.Width - panelWidth) / 2;
            double panelY = (window.Height - panelHeight) / 2;
            
            DrawPanel(panelX, panelY, panelWidth, panelHeight, window);
            
            DrawTextWithShadow("GAME OVER", 36, panelX + panelWidth/2 - 100, panelY + 40, ACCENT_COLOR, SplashKit.ColorBlack(), window);
            DrawText("Press ESC to return to menu", 16, panelX + panelWidth/2 - 110, panelY + 100, TEXT_PRIMARY, window);
            
            Rectangle backToMenuButton = new Rectangle() 
            { 
                X = panelX + panelWidth/2 - 75, 
                Y = panelY + 140, 
                Width = 150, 
                Height = 40 
            };
            MenuFromGameButton = backToMenuButton;
            DrawGameButton("Back to Menu", backToMenuButton, BUTTON_NORMAL, window);
        }
        
        private void DrawPanel(double x, double y, double width, double height, Window window)
        {
            window.FillRectangle(SplashKit.RGBColor(8, 8, 8), x + 4, y + 4, width, height);
            window.FillRectangle(PANEL_BACKGROUND, x, y, width, height);
            window.DrawRectangle(PANEL_BORDER, x, y, width, height);
            window.DrawRectangle(SplashKit.RGBColor(80, 90, 110), x + 1, y + 1, width - 2, height - 2);
        }
        
        private void DrawMenuButton(string text, Rectangle rect, Color color, Window window)
        {
            bool isHovered = SplashKit.PointInRectangle(SplashKit.MousePosition(), rect);
            Color baseColor = isHovered ? BUTTON_HOVER : color;
            Color shadowColor = SplashKit.RGBColor(8, 8, 8);

            window.FillRectangle(shadowColor, rect.X + 3, rect.Y + 3, rect.Width, rect.Height);
            window.FillRectangle(baseColor, rect.X, rect.Y, rect.Width, rect.Height);
            
            Color highlight = SplashKit.RGBColor(
                Math.Min(255, baseColor.R + 25),
                Math.Min(255, baseColor.G + 25),
                Math.Min(255, baseColor.B + 25)
            );
            window.DrawLine(highlight, rect.X, rect.Y, rect.X + rect.Width - 1, rect.Y);
            window.DrawLine(highlight, rect.X, rect.Y, rect.X, rect.Y + rect.Height - 1);
            
            window.DrawRectangle(PANEL_BORDER, rect.X, rect.Y, rect.Width, rect.Height);
            
            float textX = (float)(rect.X + (rect.Width - SplashKit.TextWidth(text, _gameFont, 16)) / 2);
            float textY = (float)(rect.Y + (rect.Height - SplashKit.TextHeight(text, _gameFont, 16)) / 2);
            DrawTextWithShadow(text, 16, textX, textY, TEXT_PRIMARY, SplashKit.ColorBlack(), window);
        }
        
        private void DrawGameButton(string text, Rectangle rect, Color color, Window window)
        {
            DrawMenuButton(text, rect, color, window);
        }
        
        private void DrawText(string text, int size, double x, double y, Color color, Window window)
        {
            if (_gameFont != null)
                window.DrawText(text, color, _gameFont, size, x, y);
        }
        
        private void DrawTextWithShadow(string text, int size, double x, double y, Color textColor, Color shadowColor, Window window)
        {
            if (_gameFont != null)
            {
                window.DrawText(text, shadowColor, _gameFont, size, x + 1, y + 1);
                window.DrawText(text, textColor, _gameFont, size, x, y);
            }
        }
        
        private string GetPieceImageName(int piece)
        {
            string colorPrefix = Piece.IsColour(piece, Piece.White) ? "W" : "B";
            string pieceName = "";
            
            switch (Piece.Type(piece))
            {
                case Piece.King: pieceName = "King"; break;
                case Piece.Queen: pieceName = "Queen"; break;
                case Piece.Rook: pieceName = "Rook"; break;
                case Piece.Bishop: pieceName = "Bishop"; break;
                case Piece.Knight: pieceName = "Knight"; break;
                case Piece.Pawn: pieceName = "Pawn"; break;
            }
            
            return colorPrefix + pieceName;
        }
        
        public void ResetBoardColors()
        {
            for (int i = 0; i < 64; i++)
            {
                if (GameControl.Board[i] != null)
                {
                    int row = i / 8;
                    int col = i % 8;
                    
                    if ((row + col) % 2 == 0)
                        GameControl.Board[i].BackgroundColor = LIGHT_SQUARE;
                    else
                        GameControl.Board[i].BackgroundColor = DARK_SQUARE;
                }
            }
            
            if (RuleBook.KingInCheck && GameControl.Board[RuleBook.FriendlyKingSquare] != null)
            {
                GameControl.Board[RuleBook.FriendlyKingSquare].BackgroundColor = CHECK_HIGHLIGHT;
            }
        }
        
        public void HighlightLegalMoves(int fromSquare, List<Move> legalMoves)
        {
            foreach (Move move in legalMoves)
            {
                if (move.MoveFrom == fromSquare && GameControl.Board[move.MoveTo] != null)
                {
                    GameControl.Board[move.MoveTo].BackgroundColor = LEGAL_MOVE;
                }
            }
        }
        
        public void HighlightLastMove(int from, int to)
        {
            if (GameControl.Board[from] != null)
                GameControl.Board[from].BackgroundColor = LAST_MOVE_FROM;
            if (GameControl.Board[to] != null)
                GameControl.Board[to].BackgroundColor = LAST_MOVE_TO;
        }
        
        public void HighlightSelectedSquare(int square)
        {
            if (GameControl.Board[square] != null)
                GameControl.Board[square].BackgroundColor = SELECTED_SQUARE;
        }

        private void DrawTextInputBox(string text, Rectangle rect, TextInputManager.ActiveField field, Window window)
        {
            window.FillRectangle(SplashKit.RGBColor(30, 30, 30), rect.X, rect.Y, rect.Width, rect.Height);
            window.DrawRectangle(PANEL_BORDER, rect.X, rect.Y, rect.Width, rect.Height);

            bool isActive = TextInputManager.CurrentField == field;
            string displayText = isActive ? TextInputManager.CurrentText : text;

            DrawText(displayText, 16, rect.X + 10, rect.Y + 8, TEXT_PRIMARY, window);

            if (isActive && TextInputManager.ShouldDrawCursor())
            {
                if (_gameFont != null)
                {
                    float textWidth = SplashKit.TextWidth(displayText, _gameFont, 16);
                    window.FillRectangle(TEXT_PRIMARY, rect.X + 10 + textWidth, rect.Y + 6, 2, rect.Height - 12);
                }
            }
        }
    }
} 