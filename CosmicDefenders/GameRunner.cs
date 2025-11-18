using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace CosmicDefenders;

internal class GameRunner
{
    RenderWindow _window;
    public GameRunner()
    {

        _window = new RenderWindow(new VideoMode(800, 600), "Cosmic Defenders");
        _window.Closed += (_, __) => _window.Close();
        _window.SetFramerateLimit(60);
        _window.SetVerticalSyncEnabled(true);

        // Subscription to keyboard events
        _window.KeyPressed += (sender, e) =>
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                _window.Close();
            }
        };
    }

    public void Run()
    {
        Music music = new Music("Assets/MainTheme.mp3");
        music.Loop = true;
        music.Volume = 50;
        music.Play();

        GameState gameState = new GameState((int)_window.Size.Y, (int)_window.Size.X);

        while (_window.IsOpen)
        {
            gameState.Clear(_window);
            gameState.Update(_window);
            gameState.Draw(_window);
        }
    }
}
