using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmicDefenders.Constants;
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

        //GameState gameState = new GameState((int)_window.Size.Y, (int)_window.Size.X);
        IGameState gameState = new TitleScreenState((int)_window.Size.X);
        int state = States.TITLE_SCREEN;

        while (_window.IsOpen)
        {
            if (state == States.EXIT)
            {
                _window.Close();
                break;
            }
            else if (state == States.GAME_SCREEN && gameState is not GameState)
            {
                gameState = new GameState((int)_window.Size.Y, (int)_window.Size.X);
            }
            else if (state == States.TITLE_SCREEN && gameState is not TitleScreenState)
            {
                gameState = new TitleScreenState((int)_window.Size.X);
            }
            else if (state == States.GAME_OVER_SCREEN && gameState is not GameOverScreenState)
            {
                gameState = new GameOverScreenState((int)_window.Size.X);
            }
            else if (state == States.SCORE_SCREEN)
            {
                //gameState = new ScoreScreenState((int)_window.Size.X, (int)_window.Size.Y);
            }

            gameState.Clear(_window);
            gameState.Update(_window);
            gameState.Draw(_window);
            state = gameState.GetState();
        }
    }
}
