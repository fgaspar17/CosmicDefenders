using System;
using System.Collections.Generic;
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
    private bool rightDirection = true;
    public void CreateEnemies(float windowHeigh, float windowWidth)
    {
        Enemies = new List<List<IEnemy>>();

        // 5 rows, 11 columns

        AddEnemyRow<EnemyYellow>(Enemies, 11, (int)windowWidth, y: 100);
        AddEnemyRow<EnemyRed>(Enemies, 11, (int)windowWidth, y: 150);
        AddEnemyRow<EnemyGreen>(Enemies, 11, (int)windowWidth, y: 200);
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

    internal bool CollidesWith(SpaceShipBullet bullet)
    {
        bool collisionDetected = false;
        foreach (var enemyRow in Enemies)
        {
            for (int i = 0; i < enemyRow.Count; i++)
            {
                IEnemy? enemy = enemyRow[i];
                if ((bullet.PositionX >= enemy.PositionX && bullet.PositionX <= enemy.PositionX + enemy.Width)
                    && bullet.PositionY <= enemy.PositionY && bullet.PositionY >= enemy.PositionY - enemy.Height)
                {
                    enemyRow.RemoveAt(i);
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

        for (int col = 1; col < columns; col++)
        {
            enemyInstance = new T();
            startX += enemyWidth + 10;
            enemyInstance.PositionEnemy(startX, y);
            enemyRow.Add(enemyInstance);
        }
        enemies.Add(enemyRow);
    }

    internal void Update(int maxRight)
    {
        bool oldDirection = rightDirection;
        if (rightDirection)
        {
            foreach (var enemyRow in Enemies)
            {
                foreach (var enemy in enemyRow)
                {
                    enemy.PositionEnemy(enemy.PositionX + 2, enemy.PositionY);
                    if (enemy.PositionX + enemy.Width >= maxRight)
                    {
                        rightDirection = false;
                    }
                }
            }
        }
        else
        {
            foreach (var enemyRow in Enemies)
            {
                foreach (var enemy in enemyRow)
                {
                    enemy.PositionEnemy(enemy.PositionX - 2, enemy.PositionY);
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
                }
            }
        }
    }
}