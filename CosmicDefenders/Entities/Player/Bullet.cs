using SFML.Graphics;
using SFML.System;

namespace CosmicDefenders.Entities.Player;

internal class Bullet
{
    private Sprite Sprite { get; set; }
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

    public Bullet(float positionX, float positionY)
    {
        // Painting the Spacecraft
        Texture texture = new(Path.Combine("Assets", "bullet.png"));
        Sprite = new(texture);
        Sprite.Position = new Vector2f(positionX, positionY);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Sprite);
    }
}
