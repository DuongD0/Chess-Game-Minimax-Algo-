using SplashKitSDK;
using System;

namespace Chess
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            // Initialize SplashKit window
            Window gameWindow = SplashKit.OpenWindow("Chess Game", 1000, 800);
            
            // Initialize game systems
            GameState.InitializeTimers();
            
            // Create chess game instance
            ChessGame chessGame = new ChessGame(gameWindow);
            
            // Main game loop
            while (!gameWindow.CloseRequested && !chessGame.QuitGame)
            {
                SplashKit.ProcessEvents();
                
                chessGame.Update();
                
                gameWindow.Clear(SplashKit.ColorWhite());
                chessGame.Draw();
                gameWindow.Refresh(60);
            }
            
            chessGame.ExitGame();
            gameWindow.Close();
        }
    }
}
