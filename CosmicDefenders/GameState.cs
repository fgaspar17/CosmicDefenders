using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public GameState()
    {
        _window = new RenderWindow(new VideoMode(800, 600), "Cosmic Defenders");
        _window.Closed += (_, __) => _window.Close();
        _window.SetFramerateLimit(60);
        _window.SetVerticalSyncEnabled(true);

        _spaceShip = new SpaceShip();

        // Subscription to keyboard events
        _window.KeyPressed += (sender, e) =>
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                _window.Close();
            }
            else if (e.Code == Keyboard.Key.Left)
            {
                _spaceShip.PositionX = _spaceShip.PositionX - 10;
            }
            else if (e.Code == Keyboard.Key.Right)
            {
                _spaceShip.PositionX = _spaceShip.PositionX + 10;
            }
        };
    }

    public void Run()
    {
        bool right = true;

        while (_window.IsOpen)
        {
            _window.DispatchEvents();
            _window.Clear(Color.Black);

            _window.Draw(_spaceShip.Sprite);
            var debugRect = GetDebugRectangle(_spaceShip.Sprite);
            _window.Draw(debugRect);

            EnemyWaveManager waveManager = new EnemyWaveManager();
            waveManager.CreateEnemies(_window.Size.Y, _window.Size.X);
            waveManager.Draw(_window);

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
