using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace Chess
{
    public class InputHandler
    {
        private int selectedSquare = -1;
        private const int TILE_SIZE = 75;
        private const int BOARD_START_X = 40;
        private const int BOARD_START_Y = 40;
        private GameRenderer _renderer;
        private ChessGame _chessGame;
        
        public InputHandler(ChessGame game, GameRenderer renderer)
        {
            _renderer = renderer;
            _chessGame = game;
        }
        
        public bool CheckMenuButton()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if (IsPointInRectangle(SplashKit.MousePosition(), _renderer.MenuFromGameButton))
                {
                    return true;
                }
            }
            return false;
        }

        public void Update()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Point2D mousePos = SplashKit.MousePosition();
                HandleMouseClick(mousePos);
            }
            
            if (SplashKit.KeyTyped(KeyCode.EscapeKey))
            {
                DeselectPiece();
            }
        }
        
        public void UpdateMenu()
        {
            if (SplashKit.KeyTyped(KeyCode.Num1Key))
            {
                _chessGame.StartSinglePlayerGame();
                return;
            }
            if (SplashKit.KeyTyped(KeyCode.Num2Key))
            {
                _chessGame.StartTwoPlayerGame();
                return;
            }
            if (SplashKit.KeyTyped(KeyCode.Num3Key))
            {
                _chessGame.GoToState(GameStateEnum.Options);
                return;
            }
            if (SplashKit.KeyTyped(KeyCode.Num4Key) || SplashKit.KeyTyped(KeyCode.EscapeKey))
            {
                _chessGame.ExitGame();
                return;
            }
            
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Point2D mousePos = SplashKit.MousePosition();
                HandleMenuClick(mousePos);
            }
        }
        
        public void UpdateOptions()
        {
            if (SplashKit.KeyTyped(KeyCode.EscapeKey))
            {
                _chessGame.GoToState(GameStateEnum.Menu);
            }
            
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Point2D mousePos = SplashKit.MousePosition();
                HandleOptionsClick(mousePos);
            }
        }
        
        private void HandleMouseClick(Point2D mousePos)
        {
            int clickedSquare = GetSquareFromPosition(mousePos);
            
            if (clickedSquare >= 0 && clickedSquare < 64)
            {
                HandleBoardClick(clickedSquare);
                return;
            }
            
            HandleGameUIClick(mousePos);
        }
        
        private void HandleMenuClick(Point2D mousePos)
        {
            if (IsPointInRectangle(mousePos, _renderer.MenuButton1))
            {
                _chessGame.StartSinglePlayerGame();
            }
            else if (IsPointInRectangle(mousePos, _renderer.MenuButton2))
            {
                _chessGame.StartTwoPlayerGame();
            }
            else if (IsPointInRectangle(mousePos, _renderer.MenuButton3))
            {
                _chessGame.GoToState(GameStateEnum.Options);
            }
            else if (IsPointInRectangle(mousePos, _renderer.MenuButton4))
            {
                _chessGame.ExitGame();
            }
        }
        
        private void HandleOptionsClick(Point2D mousePos)
        {
            if (IsPointInRectangle(mousePos, _renderer.BackButton))
            {
                _chessGame.GoToState(GameStateEnum.Menu);
            }
        }
        
        private void HandleGameUIClick(Point2D mousePos)
        {
            if (IsPointInRectangle(mousePos, _renderer.ResignButton))
            {
                GameControl.EndGame(GameControl.OpposingSide());
            }
            else if (IsPointInRectangle(mousePos, _renderer.MenuFromGameButton))
            {
                _chessGame.GoToState(GameStateEnum.Menu);
            }
        }
        
        private bool IsPointInRectangle(Point2D point, Rectangle rect)
        {
            return point.X >= rect.X && point.X <= rect.X + rect.Width &&
                   point.Y >= rect.Y && point.Y <= rect.Y + rect.Height;
        }
        
        private void HandleBoardClick(int square)
        {
            if (GameControl.Board == null || GameControl.Board[square] == null)
                return;
                
            int pieceOnSquare = GameControl.Board[square].PieceOnSquare;
            
            if (selectedSquare == -1)
            {
                if (pieceOnSquare != 0 && Piece.IsColour(pieceOnSquare, GameControl.sideToMove))
                {
                    SelectPiece(square);
                }
            }
            else
            {
                if (square == selectedSquare)
                {
                    DeselectPiece();
                }
                else if (pieceOnSquare != 0 && Piece.IsColour(pieceOnSquare, GameControl.sideToMove))
                {
                    SelectPiece(square);
                }
                else
                {
                    AttemptMove(selectedSquare, square);
                }
            }
        }
        
        private void SelectPiece(int square)
        {
            selectedSquare = square;
            
            _renderer.ResetBoardColors();
            _renderer.HighlightSelectedSquare(square);
            
            _renderer.HighlightLegalMoves(selectedSquare, GameState.CurrentLegalMoves);
        }
        
        private void DeselectPiece()
        {
            selectedSquare = -1;
            _renderer.ResetBoardColors();
        }
        
        private void AttemptMove(int fromSquare, int toSquare)
        {
            foreach (Move move in GameState.CurrentLegalMoves)
            {
                if (move.MoveFrom == fromSquare && move.MoveTo == toSquare)
                {
                    GameControl.Move(move);
                    DeselectPiece();
                    return;
                }
            }
            
            AudioManager.PlayIllegalMoveSound();
            DeselectPiece();
        }
        
        private int GetSquareFromPosition(Point2D position)
        {
            double x = position.X - BOARD_START_X;
            double y = position.Y - BOARD_START_Y;
            
            if (x < 0 || y < 0 || x >= 8 * TILE_SIZE || y >= 8 * TILE_SIZE)
                return -1;
                
            int col = (int)(x / TILE_SIZE);
            int row = (int)(y / TILE_SIZE);
            
            return row * 8 + col;
        }
        
        public bool IsMouseOverBoard(Point2D mousePos)
        {
            return mousePos.X >= BOARD_START_X && 
                   mousePos.X <= BOARD_START_X + 8 * TILE_SIZE &&
                   mousePos.Y >= BOARD_START_Y && 
                   mousePos.Y <= BOARD_START_Y + 8 * TILE_SIZE;
        }
    }
} 