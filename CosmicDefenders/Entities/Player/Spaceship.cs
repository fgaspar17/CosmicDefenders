using System.Diagnostics;
using SFML.Graphics;
using SFML.System;

namespace CosmicDefenders.Entities.Player;

internal class SpaceShip
{
    private Sprite Sprite { get; set; }
    private Stopwatch ShotTimer { get; } = Stopwatch.StartNew();

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

    public SpaceShip() : this(360, 500) { }

    public SpaceShip(int positionX, int positionY)
    {
        // Painting the Spacecraft
        Texture texture = new(Path.Combine("Assets", "DurrrSpaceShip.png"));
        Sprite = new(texture);
        Sprite.Position = new Vector2f(positionX, positionY);
        ShotTimer.Start();
    }

    public bool TryShoot(out SpaceShipBullet bullet)
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
        window.Draw(Sprite);
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
}