using System.Diagnostics;
using CosmicDefenders.Constants;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace CosmicDefenders;

internal class GameOverScreenState : IGameState
{
    Font _font;
    Font _fontPressEnter;
    Text _gameOverText;
    Text _pressEnterText;
    bool _nextScreen = false;
    Clock _clock;

    public GameOverScreenState(int windowWidth)
    {

        _font = new Font("Assets/SairaStencilOne-Regular.ttf");
        _fontPressEnter = new Font("Assets/Doto-Black.ttf");
        _clock = new Clock();
        InitializeTitleText(windowWidth);
        InitializePressAnyButtonText(windowWidth);
    }

    private void InitializeTitleText(int width)
    {
        _gameOverText = new Text("GAME OVER", _font, 40);
        _gameOverText.FillColor = Color.White;
        long x = width / 2;
        _gameOverText.Origin = new Vector2f(_gameOverText.GetGlobalBounds().Width / 2, _gameOverText.GetGlobalBounds().Height / 2);
        _gameOverText.Position = new Vector2f(x, 170);
    }

    private void InitializePressAnyButtonText(int width)
    {
        _pressEnterText = new Text("PRESS ENTER TO CONTINUE", _fontPressEnter, 20);
        _pressEnterText.FillColor = Color.White;
        long x = width / 2;
        _pressEnterText.Origin = new Vector2f(_pressEnterText.GetGlobalBounds().Width / 2, _pressEnterText.GetGlobalBounds().Height / 2);
        _pressEnterText.Position = new Vector2f(x, 300);
    }

    public void Clear(RenderWindow window)
    {
        window.Clear(Color.Black);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(_gameOverText);
        if (_clock.ElapsedTime.AsSeconds() % 1 < 0.5f)
            window.Draw(_pressEnterText);
        window.Display();
    }

    public int GetState()
    {
        return _nextScreen ? States.TITLE_SCREEN : States.GAME_OVER_SCREEN;
    }

    public void Update(RenderWindow window)
    {
        window.DispatchEvents();
        if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
        {
            _nextScreen = true;
        }
    }
}
