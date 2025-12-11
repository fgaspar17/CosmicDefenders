using System.Diagnostics;
using CosmicDefenders.Entities.Player;
using SFML.Graphics;
using SFML.System;

namespace CosmicDefenders.Entities.Enemies;

internal class EnemyYellowEyeBoss
{
    private Sprite Sprite { get; set; }

    public float PositionX
    {
        get => Sprite.Position.X;
    }
    public float PositionY
    {
        get => Sprite.Position.Y;
    }

    public int Width { get => Sprite.TextureRect.Width; }

    public int Height { get => Sprite.TextureRect.Height; }

    public int ScoreValue => 100;

    private float _minX;
    private float _maxX;
    private bool rightDirection = true;

    private int _enemyShootingCooldown = 1_000;
    private int _enemyShootingCooldownMin = 500;
    private int _enemyShootingCooldownMax = 1_500;

    private Stopwatch EnemyShootTimer { get; set; } = new Stopwatch();
    public int Life { get; private set; } = 10;

    public EnemyYellowEyeBoss(float windowWidth, float margin, float y)
    {
        Texture texture = new(Path.Combine("Assets", "YellowEyeBoss.png"));
        IntRect textureRect = new(0, 0, 96, 96);
        Sprite = new(texture, textureRect);
        var bounds = Sprite.GetLocalBounds();
        Sprite.Origin = new Vector2f(bounds.Width / 2, bounds.Height / 2);
        Sprite.Rotation = 90f;
        _minX = margin;
        _maxX = windowWidth - margin;
        PositionEnemy((_maxX + _minX) / 2, y);

        EnemyShootTimer.Start();
    }

    public void PositionEnemy(float x, float y)
    {
        Sprite.Position = new Vector2f(x, y);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Sprite);
    }

    public void Update(List<EnemyYellowBullet> enemyYellowBullets)
    {
        if (_enemyShootingCooldown <= EnemyShootTimer!.ElapsedMilliseconds)
        {
            if (TryShoot(out IBullet? bullet))
                enemyYellowBullets.Add((bullet as EnemyYellowBullet)!);
            EnemyShootTimer.Restart();
            _enemyShootingCooldown = GetRandomShootingCooldown();
        }


        if (rightDirection)
        {
            PositionEnemy(Sprite.Position.X + 2, Sprite.Position.Y);
            if (Sprite.Position.X >= _maxX)
            {
                rightDirection = false;
            }
        }
        else
        {
            PositionEnemy(Sprite.Position.X - 2, Sprite.Position.Y);
            if (Sprite.Position.X <= _minX)
            {
                rightDirection = true;
            }
        }
    }

    internal bool CollidesWith(SpaceShipBullet bullet, out int score)
    {
        score = 0;
        bool collisionDetected = false;
        EnemyYellowEyeBoss? enemy = this;
        if (Sprite.GetGlobalBounds().Contains(bullet.PositionX, bullet.PositionY))
        {
            Life--;
            if (Life <= 0)
                score += enemy.ScoreValue;
            
            collisionDetected = true;
        }

        return collisionDetected;
    }

    public bool TryShoot(out IBullet? bullet)
    {
        bullet = new EnemyYellowBullet(PositionX + ((float)this.Sprite.GetGlobalBounds().Width) / 2 - 10, PositionY + Height);
        return true;
    }

    private int GetRandomShootingCooldown()
    {
        return RandomSingleton.Instance.Next(_enemyShootingCooldownMin, _enemyShootingCooldownMax);
    }
}
