#region
using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;
using SFMLStart.Data;
using SFMLStart.Utilities;
using SFML.System;
#endregion

namespace SFMLStart
{
    public class GameWindow
    {
        private readonly int _height;
        private readonly List<string> _inputs = new List<string>();
        private readonly Keyboard.Key[] _keyCodes = (Keyboard.Key[]) Enum.GetValues(typeof (Keyboard.Key));
        private readonly Mouse.Button[] _mouseButtons = (Mouse.Button[]) Enum.GetValues(typeof (Mouse.Button));
        private readonly PerformanceStopwatch _stopwatch;
        private readonly int _width;
        private float _frameTime;
        private Game _game;
        private bool _hasFocus, _running;

        public GameWindow(int mScreenWidth, int mScreenHeight, int mPixelMultiplier)
        {
            _stopwatch = new PerformanceStopwatch();
            _width = mScreenWidth;
            _height = mScreenHeight;

            RenderWindow = new RenderWindow(new VideoMode((uint) _width, (uint) _height), "", Styles.Default);
            RenderWindow.SetVerticalSyncEnabled(false);
            if (Settings.Framerate.IsLimited) RenderWindow.SetFramerateLimit((uint) Settings.Framerate.Limit);
            RenderWindow.Position = new Vector2i(400, 80);
            RenderWindow.Size = new Vector2u((uint) (mScreenWidth*mPixelMultiplier), (uint) (mScreenHeight*mPixelMultiplier));
            RenderWindow.GainedFocus += WindowGainedFocus;
            RenderWindow.LostFocus += WindowLostFocus;
        }

        public Camera Camera { get; private set; }
        public RenderWindow RenderWindow { get; private set; }
        public float FPS { get; private set; }

        public void SetGame(Game mGame)
        {
            _game = mGame;
            _game.GameWindow = this;

            Camera = new Camera(this, _width, _height);

            _hasFocus = true;
            _running = true;
        }

        private void WindowGainedFocus(object sender, EventArgs e) { _hasFocus = true; }
        private void WindowLostFocus(object sender, EventArgs e) { _hasFocus = false; }

        public void Run()
        {
            while (_running)
            {
                RenderWindow.SetActive();

				_frameTime = Settings.Frametime.IsStatic
								 ? Settings.Frametime.StaticValue
								 : (float)_stopwatch.Elapsed / 100.0f;
				
                FPS = 60/_frameTime;

                _stopwatch.Start();
                RenderWindow.Clear(Color.White);

                RunInputs();
                _game.Update(_frameTime);
                _game.Draw();

                RenderWindow.Display();
                _stopwatch.Stop();
            }
        }
        private void RunInputs()
        {
            _inputs.Clear();
            RenderWindow.DispatchEvents();

            if (_hasFocus == false) return;

            for (var i = 0; i < _keyCodes.GetLength(0); i++)
                if (Keyboard.IsKeyPressed(_keyCodes[i])) _inputs.Add(_keyCodes[i].GetType() + _keyCodes[i].ToString());
            for (var i = 0; i < _mouseButtons.GetLength(0); i++)
                if (Mouse.IsButtonPressed(_mouseButtons[i]))
                    _inputs.Add(_mouseButtons[i].GetType() + _mouseButtons[i].ToString());
        }

        internal bool IsInputCombinationDown(KeyCombination mInput) { return mInput.Inputs.All(input => _inputs.Contains(input)); }
    }
}