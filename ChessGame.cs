using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace Chess
{
    public class ChessGame
    {
        private GameStateEnum currentState;
        private GameRenderer renderer;
        private InputHandler inputHandler;
        private bool singlePlayer;
        private Window _gameWindow;
        public bool QuitGame { get; private set; }
        
        // Game state timing
        private uint lastUpdateTime;
        private uint lastSecond;
        private const uint UPDATE_INTERVAL = 16; // ~60 FPS
        
        public ChessGame(Window window)
        {
            _gameWindow = window;
            currentState = GameStateEnum.Menu;
            renderer = new GameRenderer();
            inputHandler = new InputHandler(this, renderer);
            singlePlayer = false;
            lastUpdateTime = SplashKit.CurrentTicks();
            lastSecond = SplashKit.CurrentTicks();
            QuitGame = false;
        }
        
        public void Update()
        {
            uint currentTime = SplashKit.CurrentTicks();
            
            if (currentTime - lastUpdateTime < UPDATE_INTERVAL)
                return;
                
            lastUpdateTime = currentTime;
            
            if (_gameWindow.CloseRequested)
            {
                ExitGame();
            }

            switch (currentState)
            {
                case GameStateEnum.Menu:
                    UpdateMenu();
                    break;
                case GameStateEnum.Options:
                    UpdateOptions();
                    break;
                case GameStateEnum.Playing:
                    UpdateGame();
                    break;
            }
        }
        
        public void Draw()
        {
            switch (currentState)
            {
                case GameStateEnum.Menu:
                    renderer.DrawMenu(_gameWindow);
                    break;
                case GameStateEnum.Options:
                    renderer.DrawOptions(_gameWindow);
                    break;
                case GameStateEnum.Playing:
                    renderer.DrawGame(_gameWindow);
                    break;
            }
        }
        
        private void UpdateMenu()
        {
            inputHandler.UpdateMenu();
        }
        
        private void UpdateOptions()
        {
            inputHandler.UpdateOptions();
        }
        
        private void UpdateGame()
        {
            inputHandler.Update();
            
            if (GameState.TimeControl > 0)
            {
                UpdateTimers();
            }
            
            if (GameControl.gameEnded)
            {
                if (SplashKit.KeyTyped(KeyCode.EscapeKey) || inputHandler.CheckMenuButton())
                {
                    GoToState(GameStateEnum.Menu);
                }
            }
        }
        
        public void GoToState(GameStateEnum newState)
        {
            if (currentState != newState)
            {
                Console.WriteLine($"State transition: {currentState} -> {newState}");
                currentState = newState;
                
                if (newState == GameStateEnum.Menu)
                {
                    ResetGame();
                }
            }
        }
        
        private void ResetGame()
        {
            GameControl.SetOriginalVariables(singlePlayer);
            if (renderer != null)
            {
                renderer.ResetBoardColors();
            }
        }
        
        private void UpdateTimers()
        {
            uint currentTime = SplashKit.CurrentTicks();
            
            if (currentTime - lastSecond >= 1000)
            {
                lastSecond = currentTime;
                
                if (GameControl.sideToMove == 8 && GameState.WhiteTimeLeft > 0)
                {
                    GameState.WhiteTimeLeft--;
                    if (GameState.WhiteTimeLeft <= 0)
                    {
                        GameControl.EndGame(16); // Black wins
                    }
                }
                else if (GameControl.sideToMove == 16 && GameState.BlackTimeLeft > 0)
                {
                    GameState.BlackTimeLeft--;
                    if (GameState.BlackTimeLeft <= 0)
                    {
                        GameControl.EndGame(8); // White wins
                    }
                }
            }
        }
        
        public void StartSinglePlayerGame()
        {
            singlePlayer = true;
            StartGame();
        }
        
        public void StartTwoPlayerGame()
        {
            singlePlayer = false;
            StartGame();
        }
        
        public void ExitGame()
        {
            QuitGame = true;
        }
        
        private void StartGame()
        {
            GoToState(GameStateEnum.Playing);
            GameControl.SetOriginalVariables(singlePlayer);
            
            RuleBook.ClearLists();
            MoveStack.Clear(); 
            
            GameControl.Initialize();
            
            GameState.InitializeTimers();
            lastSecond = SplashKit.CurrentTicks();
            
            RuleBook.SideToGenerateFor = GameControl.CheckSideToMove();
            GameState.SetLegalMoves();
            
            Console.WriteLine($"Game started! {GameControl.Moves} moves played, {GameState.CurrentLegalMoves.Count} legal moves available.");
        }
    }
    
    public enum GameStateEnum
    {
        Menu,
        Options,
        Playing
    }
} 