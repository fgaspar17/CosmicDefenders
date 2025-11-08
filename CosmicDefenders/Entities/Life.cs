using System.Diagnostics;
using SFML.Graphics;
using SFML.System;

namespace CosmicDefenders.Entities;

internal class Life
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

    public Life(int positionX, int positionY)
    {
        // Painting the Spacecraft
        Texture texture = new(Path.Combine("Assets", "HeartPixelArt.png"));
        Sprite = new(texture);
        Sprite.Position = new Vector2f(positionX, positionY);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Sprite);
    }
}