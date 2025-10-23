using SFML.Graphics;
using SFML.System;

namespace CosmicDefenders.Entities.Enemies;

internal class EnemyYellow : IEnemy
{
    private Sprite Enemy { get; set; }
    public int Width { get => Enemy.TextureRect.Width; }
    public EnemyYellow()
    {
        Texture texture = new(Path.Combine("Assets", "yellow.png"));
        Enemy = new(texture);
    }

    public void PositionEnemy(float x, float y)
    {
        Enemy.Position = new Vector2f(x, y);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Enemy);
    }
}