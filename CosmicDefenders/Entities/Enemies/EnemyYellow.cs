using SFML.Graphics;
using SFML.System;

namespace CosmicDefenders.Entities.Enemies;

internal class EnemyYellow : IEnemy
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

    public int ScoreValue => 20;

    public EnemyYellow()
    {
        Texture texture = new(Path.Combine("Assets", "yellow.png"));
        Sprite = new(texture);
    }

    public void PositionEnemy(float x, float y)
    {
        Sprite.Position = new Vector2f(x, y);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Sprite);
    }
}