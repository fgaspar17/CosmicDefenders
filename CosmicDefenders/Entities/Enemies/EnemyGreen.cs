using SFML.Graphics;
using SFML.System;

namespace CosmicDefenders.Entities.Enemies;

internal class EnemyGreen : IEnemy
{
    private Sprite Sprite { get; set; }
    public int Width { get => Sprite.TextureRect.Width; }
    public EnemyGreen()
    {
        Texture texture = new(Path.Combine("Assets", "green.png"));
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