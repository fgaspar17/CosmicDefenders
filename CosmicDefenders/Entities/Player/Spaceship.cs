using System.Diagnostics;
using CosmicDefenders.Entities.Enemies;
using SFML.Graphics;
using SFML.System;

namespace CosmicDefenders.Entities.Player;

internal class SpaceShip : IShooter
{
    private Sprite Sprite { get; set; }
    private Shader? Shader { get; set; }
    private Stopwatch ShotTimer { get; } = Stopwatch.StartNew();
    private float _damageFlash = 0f;
    Clock Clock { get; } = new();

    public float PositionX
    {
        get => Sprite.Position.X;
        set => Sprite.Position = new Vector2f(value, Sprite.Position.Y);
    }

    public float PositionY
    {
        get => Sprite.Position.Y;
        set => Sprite.Position = new Vector2f(Sprite.Position.X, value);
    }

    public float Width { get => Sprite.GetGlobalBounds().Width; }
    public float Height { get => Sprite.GetGlobalBounds().Height; }

    public SpaceShip() : this(360, 500) { }

    public SpaceShip(int positionX, int positionY)
    {
        // Painting the Spacecraft
        Texture texture = new(Path.Combine("Assets", "DurrrSpaceShip.png"));
        Sprite = new(texture);
        Sprite.Position = new Vector2f(positionX, positionY);

        if (Shader.IsAvailable)
        {
            Shader = new Shader(null, null, Path.Combine("Assets", "damaged.frag"));
        }

        ShotTimer.Start();
    }

    public bool TryShoot(out IBullet? bullet)
    {
        bullet = null;

        if (ShotTimer.ElapsedMilliseconds >= 500)
        {
            ShotTimer.Restart();
            bullet = new SpaceShipBullet(PositionX + ((float)this.Sprite.GetGlobalBounds().Width) / 2 - 10, PositionY);
            return true;
        }

        return false;
    }

    public void Draw(RenderWindow window)
    {
        if (Shader.IsAvailable)
        {
            Shader!.SetUniform("time", Clock.ElapsedTime.AsMilliseconds());
            Shader.SetUniform("damageFlash", _damageFlash);
            window.Draw(Sprite, new RenderStates(Shader));
            if (_damageFlash > 0f)
            {
                _damageFlash -= 0.1f;
                if (_damageFlash < 0f)
                    _damageFlash = 0f;
            }
        }
        else
        {
            window.Draw(Sprite);
        }
    }

    public void DrawDebug(RenderWindow window)
    {
        if (Sprite != null)
        {
            var bounds = Sprite.GetGlobalBounds();
            var debugRect = new RectangleShape(new Vector2f(bounds.Width, bounds.Height));
            debugRect.Position = new Vector2f(bounds.Left, bounds.Top);
            debugRect.OutlineColor = Color.Red;
            debugRect.OutlineThickness = 1;
            debugRect.FillColor = Color.Transparent;
            window.Draw(debugRect);
        }
    }

    internal bool CollidesWith(EnemyYellowBullet bullet, out int life)
    {
        life = 0;
        bool collisionDetected = false;

        SpaceShip? ship = this;
        if ((bullet.PositionX >= ship.PositionX && bullet.PositionX <= ship.PositionX + ship.Width)
            && bullet.PositionY >= ship.PositionY && bullet.PositionY <= ship.PositionY + ship.Height)
        {
            life += 1;
            collisionDetected = true;
            _damageFlash = 2f;
        }

        return collisionDetected;
    }
}