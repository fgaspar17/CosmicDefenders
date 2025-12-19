using CosmicDefenders.Constants;
using CosmicDefenders.Entities;
using CosmicDefenders.Entities.Enemies;
using CosmicDefenders.Entities.Player;
using CosmicDefenders.Helpers;
using CosmicDefenders.Levels;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace CosmicDefenders;

internal class GameState : IGameState
{
    SpaceShip _spaceShip;
    List<SpaceShipBullet> _playerBullets;
    List<Asteroid> _asteroids;
    List<SpaceShipBulletExplosion> _explosions;
    List<EnemyYellowBullet> _enemyBullets;
    EnemyYellowEyeBoss _boss;
    int _score = 0;
    int _life = 3;
    Font _font;
    EnemyWaveManager _waveManager;
    int _asteroidY = 380;
    float _enemyY;
    LevelManager _level;
    int _height, _width;
    bool _win = false;

    public GameState(int height, int width)
    {
        _height = height;
        _width = width;
        _font = new Font("Assets/SairaStencilOne-Regular.ttf");
        _spaceShip = new SpaceShip();


        _level = new LevelManager();
    }

    public void Clear(RenderWindow window)
    {
        window.Clear(Color.Black);
    }

    public void Update(RenderWindow window)
    {
        window.DispatchEvents();

        KeyboardInput(window);

        UpdateLevel(_level);

        if (_win)
            return;

        UpdateAsteroids(_asteroids, _playerBullets, _enemyBullets);

        UpdateSpaceShipBullets(_waveManager);

        UpdateExplosions(_explosions);

        UpdateEnemies(_waveManager, window.Size.X);

        UpdateEnemyBullets(_enemyBullets, window.Size.Y);

    }

    private void UpdateLevel(LevelManager level)
    {
        if (level.Uninitilized || NoEnemiesRemaining(level))
        {
            level.LevelUp();
            if (level.CurrentLevel is null)
                _win = true;
            else
            {
                _playerBullets = new List<SpaceShipBullet>();
                _asteroids = new AsteroidFactory().CreateAsteroids(3, _asteroidY, _width);
                _explosions = new List<SpaceShipBulletExplosion>();
                _enemyBullets = new List<EnemyYellowBullet>();
                _waveManager = new EnemyWaveManager();
                _waveManager.CreateEnemies(_height, _width, level.CurrentLevel.EnemySpeedBoost, (int)level.CurrentLevel.EnemyNumber / 3, level.CurrentLevel.EnemyShootCooldown);
                _enemyY = _waveManager.MaxY;
                if (level.CurrentLevel.EnemyBoss)
                    _boss = new EnemyYellowEyeBoss(_width, 100, 50);
            }
        }
    }

    private void UpdateAsteroids(List<Asteroid> asteroids, List<SpaceShipBullet> playerBullets, List<EnemyYellowBullet> enemyBullets)
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            Asteroid? asteroid = asteroids[i];
            // Check collision with player bullets
            for (int j = 0; j < playerBullets.Count; j++)
            {
                SpaceShipBullet? bullet = playerBullets[j];
                bool destroyed = false;
                if (asteroid.CollidesWith(bullet, out destroyed))
                {
                    playerBullets.RemoveAt(j);
                    j--;
                    if (destroyed)
                    {
                        asteroids.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            if (i < 0 || i >= asteroids.Count)
                continue;
            // Check collision with enemy bullets
            for (int k = 0; k < enemyBullets.Count; k++)
            {
                EnemyYellowBullet? bullet = enemyBullets[k];
                bool destroyed = false;
                if (asteroid.CollidesWith(bullet, out destroyed))
                {
                    enemyBullets.RemoveAt(k);
                    k--;
                    if (destroyed)
                    {
                        asteroids.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
        }
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
        _enemyY = waveManager.MaxY;

        if (_boss != null)
        {
            _boss.Update(_enemyBullets);
            if (_boss.Life <= 0)
                _boss = null;
        }
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

            if (_boss != null && _boss.CollidesWith(bullet, out score))
            {
                _score += score;
                _playerBullets.RemoveAt(i);
                i--;
                _explosions.Add(new SpaceShipBulletExplosion(bullet.PositionX, bullet.PositionY));
            }

            bullet.Update();
        }
    }

    public void Draw(RenderWindow window)
    {
        DrawSpaceShip(window, _spaceShip);

        DrawSpaceShipBullets(window, _playerBullets);

        DrawExplosions(window, _explosions);

        DrawEnemies(window);

        DrawEnemyBullets(window);

        DrawScoreText(window, _score);

        DrawLives(window);

        DrawAsteroids(window, _asteroids);

        window.Display();
    }

    private void DrawEnemies(RenderWindow window)
    {
        _waveManager.Draw(window);
        if (_boss != null)
            _boss.Draw(window);
    }

    private void DrawAsteroids(RenderWindow window, List<Asteroid> asteroids)
    {
        foreach (var asteroid in asteroids)
        {
            asteroid.Draw(window);
        }
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
        if (_life <= 0 || _enemyY > _asteroidY)
        {
            ScoresHelper.WriteScore(_score);
            return States.GAME_OVER_SCREEN;
        }

        if (_win)
        {
            ScoresHelper.WriteScore(_score);
            return States.WIN_SCREEN;
        }

        return States.GAME_SCREEN;
    }

    private bool NoEnemiesRemaining(LevelManager level)
    {
        bool noEnemiesWave = _waveManager != null && _waveManager.Count == 0;
        bool bossExists = level.CurrentLevel.EnemyBoss;
        bool bossDead = _boss == null;

        return noEnemiesWave && (!bossExists || bossDead);
    }
}
