using SFML.Graphics;
using SFML.System;

namespace CosmicDefenders.Entities.Player;

internal class SpaceShip
{
    public Sprite Sprite { get; private set; }
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

    public SpaceShip() : this(360, 500) { }

    public SpaceShip(int positionX, int positionY)
    {
        // Painting the Spacecraft
        Texture texture = new(Path.Combine("Assets", "DurrrSpaceShip.png"));
        Sprite = new(texture);
        Sprite.Position = new Vector2f(positionX, positionY);
    }
}