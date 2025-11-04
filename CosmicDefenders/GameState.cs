using CosmicDefenders.Entities.Enemies;
using CosmicDefenders.Entities.Player;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace CosmicDefenders;

internal class GameState
{
    RenderWindow _window;
    SpaceShip _spaceShip;
    List<SpaceShipBullet> _bullets;
    int _score = 0;
    Font _font;

    public GameState()
    {
        _window = new RenderWindow(new VideoMode(800, 600), "Cosmic Defenders");
        _window.Closed += (_, __) => _window.Close();
        _window.SetFramerateLimit(60);
        _window.SetVerticalSyncEnabled(true);

        _font = new Font("Assets/SairaStencilOne-Regular.ttf");
        _spaceShip = new SpaceShip();
        _bullets = new List<SpaceShipBullet>();

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
        EnemyWaveManager waveManager = new EnemyWaveManager();
        waveManager.CreateEnemies(_window.Size.Y, _window.Size.X);

        while (_window.IsOpen)
        {
            _window.DispatchEvents();
            _window.Clear(Color.Black);

            if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
            {
                float newPositionX = _spaceShip.PositionX - 10;
                if (newPositionX > 0)
                    _spaceShip.PositionX = newPositionX;
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {
                float newPositionX = _spaceShip.PositionX + 10;
                if (newPositionX < _window.Size.X - _spaceShip.Width)
                    _spaceShip.PositionX = newPositionX;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                if (_spaceShip.TryShoot(out var spaceShipBullet))
                    _bullets.Add(spaceShipBullet);
            }

            _spaceShip.Draw(_window);
            _spaceShip.DrawDebug(_window);



            for (int i = 0; i < _bullets.Count; i++)
            {
                SpaceShipBullet? bullet = _bullets[i];
                int score = 0;
                if (bullet.PositionY < 0 || waveManager.CollidesWith(bullet, out score))
                {
                    _score += score;
                    _bullets.RemoveAt(i);
                    i--;
                    continue;
                }

                bullet.Update();
                bullet.Draw(_window);
            }

            waveManager.Update((int)_window.Size.X);
            waveManager.Draw(_window);

            Text scoreText = new Text($"Score: {_score}", _font, 20);
            scoreText.FillColor = Color.White;
            long scoreX = _window.Size.X - (int)scoreText.GetGlobalBounds().Width - 10;
            scoreText.Position = new Vector2f(scoreX, 10);
            _window.Draw(scoreText);

            _window.Display();
        }
    }

    static RectangleShape GetDebugRectangle(Sprite sprite)
    {
        var bounds = sprite.GetGlobalBounds();
        var debugRect = new RectangleShape(new Vector2f(bounds.Width, bounds.Height));
        debugRect.Position = new Vector2f(bounds.Left, bounds.Top);
        debugRect.OutlineColor = Color.Red;
        debugRect.OutlineThickness = 1;
        debugRect.FillColor = Color.Transparent;
        return debugRect;
    }
}
