using CosmicDefenders.Constants;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace CosmicDefenders;

internal class GameOverScreenState : IGameState
{
    Font _font;
    Text _gameOverText;
    Text _pressAnyButtonText;
    bool _nextScreen = false;

    public GameOverScreenState(int windowWidth)
    {

        _font = new Font("Assets/SairaStencilOne-Regular.ttf");
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
        _pressAnyButtonText = new Text("PRESS ANY BUTTON TO CONTINUE", _font, 20);
        _pressAnyButtonText.FillColor = Color.White;
        long x = width / 2;
        _pressAnyButtonText.Origin = new Vector2f(_pressAnyButtonText.GetGlobalBounds().Width / 2, _pressAnyButtonText.GetGlobalBounds().Height / 2);
        _pressAnyButtonText.Position = new Vector2f(x, 300);
    }

    public void Clear(RenderWindow window)
    {
        window.Clear(Color.Black);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(_gameOverText);
        window.Draw(_pressAnyButtonText);
        window.Display();
    }

    public int GetState()
    {
        return _nextScreen ? States.TITLE_SCREEN : States.GAME_OVER_SCREEN;
    }

    public void Update(RenderWindow window)
    {
        window.DispatchEvents();
        if (Keyboard.IsKeyPressed(Keyboard.Key.Enter) ||
                 Keyboard.IsKeyPressed(Keyboard.Key.Space) ||
                 Keyboard.IsKeyPressed(Keyboard.Key.A) ||
                 Keyboard.IsKeyPressed(Keyboard.Key.D) ||
                 Keyboard.IsKeyPressed(Keyboard.Key.W) ||
                 Keyboard.IsKeyPressed(Keyboard.Key.S) ||
                 Keyboard.IsKeyPressed(Keyboard.Key.Left) ||
                 Keyboard.IsKeyPressed(Keyboard.Key.Right) ||
                 Keyboard.IsKeyPressed(Keyboard.Key.Up) ||
                 Keyboard.IsKeyPressed(Keyboard.Key.Down))
        {
            _nextScreen = true;
        }
    }
}
