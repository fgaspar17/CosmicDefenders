using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmicDefenders.Entities.Player;
using SFML.Graphics;
using SFML.Window;

namespace CosmicDefenders.Entities.Enemies;

internal class EnemyWaveManager
{
    public List<List<IEnemy>> Enemies { get; private set; }
    public int Count { get; private set; }
    private bool rightDirection = true;
    private Stopwatch? EnemyShotTimer { get; set; }
    public float MaxY { get; set; }
    private int _enemyShootingCooldown;
    private const int _enemyShootingCooldownMax = 3_000;
    private const int _enemyShootingCooldownMin = 2_000;
    private float _speedBoost;
    public void CreateEnemies(float windowHeigh, float windowWidth, float speedBoost)
    {
        Enemies = new List<List<IEnemy>>();

        // 5 rows, 11 columns

        AddEnemyRow<EnemyYellow>(Enemies, 11, (int)windowWidth, y: 100);
        EnemyShotTimer = Stopwatch.StartNew();
        _enemyShootingCooldown = GetRandomShootingCooldown();
        AddEnemyRow<EnemyGreen>(Enemies, 11, (int)windowWidth, y: 150);
        AddEnemyRow<EnemyRed>(Enemies, 11, (int)windowWidth, y: 200);
        MaxY = 200 + 32;
        Count = 33;
        _speedBoost = speedBoost;
    }

    internal void Draw(RenderWindow window)
    {
        foreach (var enemyRow in Enemies)
        {
            foreach (var enemy in enemyRow)
            {
                enemy.Draw(window);
            }
        }
    }

    internal bool CollidesWith(SpaceShipBullet bullet, out int score, out IEnemy enemyKilled)
    {
        score = 0;
        enemyKilled = null;
        bool collisionDetected = false;
        foreach (var enemyRow in Enemies)
        {
            for (int i = 0; i < enemyRow.Count; i++)
            {
                IEnemy? enemy = enemyRow[i];
                if ((bullet.PositionX >= enemy.PositionX && bullet.PositionX <= enemy.PositionX + enemy.Width)
                    && bullet.PositionY <= enemy.PositionY && bullet.PositionY >= enemy.PositionY - enemy.Height)
                {
                    score += enemy.ScoreValue;
                    enemyKilled = enemy;
                    enemyRow.RemoveAt(i);
                    Count--;
                    i--;
                    collisionDetected = true;
                }
            }
        }

        return collisionDetected;
    }

    private void AddEnemyRow<T>(List<List<IEnemy>> enemies, int columns, int totalWidth, int y) where T : IEnemy, new()
    {
        List<IEnemy> enemyRow = new List<IEnemy>();

        IEnemy enemyInstance = new T();
        int enemyWidth = enemyInstance.Width;
        int total = columns * enemyWidth + (columns - 1) * 10;
        int startX = (totalWidth - total) / 2;
        enemyInstance.PositionEnemy(startX, y);
        enemyRow.Add(enemyInstance);

        for (int col = 1; col < columns; col++)
        {
            enemyInstance = new T();
            startX += enemyWidth + 10;
            enemyInstance.PositionEnemy(startX, y);
            enemyRow.Add(enemyInstance);
        }
        enemies.Add(enemyRow);
    }

    internal void Update(int maxRight, out List<EnemyYellowBullet> enemyYellowBullets)
    {
        enemyYellowBullets = new List<EnemyYellowBullet>();
        bool oldDirection = rightDirection;

        foreach (var enemyRow in Enemies)
        {
            List<IShooter> enemiesShooter = enemyRow.OfType<IShooter>().ToList();
            if (enemiesShooter.Count > 0)
            {
                if (_enemyShootingCooldown <= EnemyShotTimer!.ElapsedMilliseconds)
                {
                    int randomIndex = RandomSingleton.Instance.Next(0, enemiesShooter.Count);
                    if (enemiesShooter[randomIndex].TryShoot(out IBullet? bullet))
                        enemyYellowBullets.Add((bullet as EnemyYellowBullet)!);
                    EnemyShotTimer.Restart();
                    _enemyShootingCooldown = GetRandomShootingCooldown();
                }
            }

            if (rightDirection)
            {
                foreach (var enemy in enemyRow)
                {
                    enemy.PositionEnemy(enemy.PositionX + (2 + (_speedBoost * (1.00f/Count))), enemy.PositionY);
                    if (enemy.PositionX + enemy.Width >= maxRight)
                    {
                        rightDirection = false;
                    }
                }
            }
            else
            {
                foreach (var enemy in enemyRow)
                {
                    enemy.PositionEnemy(enemy.PositionX - (2 + (_speedBoost * (1.00f / Count))), enemy.PositionY);
                    if (enemy.PositionX <= 0)
                    {
                        rightDirection = true;
                    }
                }
            }

        }

        if (oldDirection != rightDirection)
        {
            foreach (var enemyRow in Enemies)
            {
                foreach (var enemy in enemyRow)
                {
                    enemy.PositionEnemy(enemy.PositionX, enemy.PositionY + 10);
                    if (enemy.PositionY + enemy.Height >= MaxY)
                    { 
                        MaxY = enemy.PositionY + enemy.Height;
                    }
                }
            }
        }
    }

    private int GetRandomShootingCooldown()
    {
        return RandomSingleton.Instance.Next(_enemyShootingCooldownMin, _enemyShootingCooldownMax);
    }
}