using SplashKitSDK;

namespace Chess
{
    public static class TextInputManager
    {
        public enum ActiveField { None, Player1, Player2, FEN }
        public static ActiveField CurrentField { get; private set; } = ActiveField.None;
        public static bool IsReadingText => CurrentField != ActiveField.None;

        private static string _currentText = "";
        private static System.Action<string>? _onComplete;
        private static double _lastBlinkTime;
        private static bool _showCursor;

        public static void StartReading(ActiveField field, string initialText, System.Action<string> onComplete, Rectangle area)
        {
            CurrentField = field;
            _currentText = initialText;
            _onComplete = onComplete;
            SplashKit.StartReadingText(area, initialText);
        }

        public static void Update()
        {
            if (!IsReadingText) return;

            _currentText = SplashKit.TextInput();

            if (SplashKit.KeyTyped(KeyCode.ReturnKey) || SplashKit.KeyTyped(KeyCode.EscapeKey))
            {
                StopReading();
            }
        }

        public static void StopReading()
        {
            if (IsReadingText)
            {
                SplashKit.EndReadingText();
                _onComplete?.Invoke(_currentText);
                CurrentField = ActiveField.None;
            }
        }

        public static string CurrentText => _currentText;
        public static bool ShouldDrawCursor()
        {
            if (SplashKit.CurrentTicks() - _lastBlinkTime > 400)
            {
                _showCursor = !_showCursor;
                _lastBlinkTime = SplashKit.CurrentTicks();
            }
            return _showCursor;
        }
    }
} 