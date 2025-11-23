using CosmicDefenders.Constants;
using CosmicDefenders.Entities;
using CosmicDefenders.Entities.Enemies;
using CosmicDefenders.Entities.Player;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace CosmicDefenders;

internal class GameState : IGameState
{
    SpaceShip _spaceShip;
    List<SpaceShipBullet> _playerBullets;
    List<SpaceShipBulletExplosion> _explosions;
    List<EnemyYellowBullet> _enemyBullets;
    int _score = 0;
    int _life = 3;
    Font _font;
    EnemyWaveManager _waveManager;

    public GameState(int height, int width)
    {
        _font = new Font("Assets/SairaStencilOne-Regular.ttf");
        _spaceShip = new SpaceShip();
        _playerBullets = new List<SpaceShipBullet>();
        _explosions = new List<SpaceShipBulletExplosion>();
        _enemyBullets = new List<EnemyYellowBullet>();
        _waveManager = new EnemyWaveManager();
        _waveManager.CreateEnemies(height, width);
    }

    public void Clear(RenderWindow window)
    {
        window.Clear(Color.Black);
    }

    public void Update(RenderWindow window)
    {
        window.DispatchEvents();

        KeyboardInput(window);

        UpdateSpaceShipBullets(_waveManager);

        UpdateExplosions(_explosions);

        UpdateEnemies(_waveManager, window.Size.X);

        UpdateEnemyBullets(_enemyBullets, window.Size.Y);

    }

    private void UpdateEnemyBullets(List<EnemyYellowBullet> enemyBullets, float height)
    {
        for (int i = 0; i < _enemyBullets.Count; i++)
        {
            EnemyYellowBullet? bullet = _enemyBullets[i];
            int life = 0;
            if (bullet.PositionY > height ||
        _spaceShip.CollidesWith(bullet, out life))
            {
                _life -= life;
                _enemyBullets.RemoveAt(i);
                i--;
                continue;
            }
            bullet.Update();
        }
    }

    private void UpdateEnemies(EnemyWaveManager waveManager, float width)
    {
        waveManager.Update((int)width, out List<EnemyYellowBullet> enemyBullets);
        _enemyBullets.AddRange(enemyBullets);
    }

    private void UpdateExplosions(List<SpaceShipBulletExplosion> explosions)
    {
        for (int i = 0; i < explosions.Count; i++)
        {
            SpaceShipBulletExplosion? explosion = explosions[i];
            explosion.Update();
            if (explosion.IsFinished)
            {
                explosions.RemoveAt(i);
                i--;
                continue;
            }
        }
    }

    private void UpdateSpaceShipBullets(EnemyWaveManager waveManager)
    {
        for (int i = 0; i < _playerBullets.Count; i++)
        {
            SpaceShipBullet? bullet = _playerBullets[i];
            int score = 0;
            if (bullet.PositionY < 0)
            {
                _playerBullets.RemoveAt(i);
                i--;
                continue;
            }

            if (waveManager.CollidesWith(bullet, out score, out IEnemy enemyKilled))
            {
                _score += score;
                _playerBullets.RemoveAt(i);
                i--;
                _explosions.Add(new SpaceShipBulletExplosion(enemyKilled.PositionX + enemyKilled.Width / 2, enemyKilled.PositionY + enemyKilled.Height / 2));
            }

            bullet.Update();
        }
    }

    public void Draw(RenderWindow window)
    {
        DrawSpaceShip(window, _spaceShip);

        DrawSpaceShipBullets(window, _playerBullets);

        DrawExplosions(window, _explosions);

        _waveManager.Draw(window);

        DrawEnemyBullets(window);

        DrawScoreText(window, _score);

        DrawLives(window);

        window.Display();
    }

    private void DrawSpaceShipBullets(RenderWindow window, List<SpaceShipBullet> playerBullets)
    {
        foreach (var bullet in playerBullets)
        {
            bullet.Draw(window);
        }
    }

    private void DrawLives(RenderWindow window)
    {
        for (int i = 0; i < _life; i++)
        {
            Life lifeIcon = new Life(10 + i * 18, 10);
            lifeIcon.Draw(window);
        }
    }

    private void DrawScoreText(RenderWindow window, int score)
    {
        Text scoreText = new Text($"Score: {score}", _font, 20);
        scoreText.FillColor = Color.White;
        long scoreX = window.Size.X - (int)scoreText.GetGlobalBounds().Width - 10;
        scoreText.Position = new Vector2f(scoreX, 10);
        window.Draw(scoreText);
    }

    private void DrawEnemyBullets(RenderWindow window)
    {
        for (int i = 0; i < _enemyBullets.Count; i++)
        {
            EnemyYellowBullet? bullet = _enemyBullets[i];
            bullet.Draw(window);
        }
    }

    private void DrawSpaceShip(RenderWindow window, SpaceShip spaceShip)
    {
        _spaceShip.Draw(window);
        _spaceShip.DrawDebug(window);
    }

    private void DrawExplosions(RenderWindow window, List<SpaceShipBulletExplosion> explosions)
    {
        for (int i = 0; i < explosions.Count; i++)
        {
            SpaceShipBulletExplosion? explosion = explosions[i];
            explosion.Draw(window);
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

    private void KeyboardInput(RenderWindow window)
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {
            float newPositionX = _spaceShip.PositionX - 10;
            if (newPositionX > 0)
                _spaceShip.PositionX = newPositionX;
        }
        else if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {
            float newPositionX = _spaceShip.PositionX + 10;
            if (newPositionX < window.Size.X - _spaceShip.Width)
                _spaceShip.PositionX = newPositionX;
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
        {
            if (_spaceShip.TryShoot(out IBullet? spaceShipBullet))
                _playerBullets.Add((spaceShipBullet as SpaceShipBullet)!);
        }
    }

    public int GetState()
    {
        if (_life <= 0)
            return States.GAME_OVER_SCREEN;

        return States.GAME_SCREEN;
    }
}
